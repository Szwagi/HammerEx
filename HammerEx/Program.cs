using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;

namespace HammerEx
{
    public struct TeleportDestination
    {
        public float px, py, pz;
        public float rx, ry, rz;
    }

    public static class Program
    {
        public const char ESC = (char)0x1B;
        
        public static int UniqueInt = 1;

        public static Dictionary<string, TeleportDestination> TeleportDestinations = new Dictionary<string, TeleportDestination>();

        public static bool BhopFilter = false;
        public static HashSet<int> SequentialFilters = new HashSet<int>();

        public static void WriteError(string msg)
        {
            var prevColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(msg);
            Console.ForegroundColor = prevColor;
        }

        public static void Main(string[] args)
        {
            try {
                CultureInfo.CurrentCulture = new CultureInfo("en-US");

                if (args.Length != 1) {
                    WriteError("Usage: HammerEx kz_mymap.vmf");
                    return;
                }

                var inputFilename = args[0] + ".vmf";
                var outputFilename = args[0] + ".ex";

                File.Delete(outputFilename);
                if (!File.Exists(inputFilename)) {
                    WriteError("Input file does not exist.");
                    return;
                }

                var vdfContent = "vmf { " + File.ReadAllText(inputFilename) + " }";
                var vdf = VdfConvert.Deserialize(vdfContent);

                CacheStuff(vdf);

                StringBuilder sb = new StringBuilder();
                if (!ProcessMap(vdf, sb)) {
                    return;
                }

                File.WriteAllText(outputFilename, sb.ToString());
            }
            catch (Exception ex) {
                WriteError(ex.Message);
            }
        }

        public static void CacheStuff(VProperty vdf)
        {
            foreach (var prop in vdf.Value.Cast<VProperty>()) {
                if (prop.Key == "entity") {
                    switch (prop.Value["classname"].ToString()) {
                        case "info_teleport_destination":
                            CacheInfoTeleportDestination(prop);
                            break;
                    }
                }
            }
        }

        public static void CacheInfoTeleportDestination(VProperty prop)
        {
            var targetName = prop.Value["targetname"].ToString();
            if (targetName.Length == 0)
                return;

            var origin = prop.Value["origin"].ToString().Split(' ');
            var angles = prop.Value["angles"].ToString().Split(' ');

            var tp = new TeleportDestination();
            tp.px = float.Parse(origin[0]);
            tp.py = float.Parse(origin[1]);
            tp.pz = float.Parse(origin[2]);
            tp.rx = float.Parse(angles[0]);
            tp.ry = float.Parse(angles[1]);
            tp.rz = float.Parse(angles[2]);

            TeleportDestinations.Add(targetName, tp);
        }

        public static void AppendSolids(VProperty vdf, StringBuilder sb)
        {
            foreach (var prop in vdf.Value.Cast<VProperty>()) {
                if (prop.Key == "solid") {
                    sb.AppendLine(prop.ToString());
                }
            }
        }

        public static bool ProcessMap(VProperty vdf, StringBuilder sb)
        {
            var success = true;
            foreach (var prop in vdf.Value.Cast<VProperty>()) {
                UniqueInt++;

                if (prop.Key == "world") {
                    ProcessWorld(prop, sb);
                }
                else if (prop.Key == "entity") {
                    switch (prop.Value["classname"].ToString()) {
                        case "trigger_reset":
                            ProcessTriggerReset(prop, sb);
                            break;
                        case "trigger_bhop": 
                            if (!ProcessTriggerBhop(prop, sb))
                                success = false;
                            break;
                        case "trigger_bhop_single":
                            if (!ProcessTriggerBhopSingle(prop, sb))
                                success = false;
                            break;
                        case "trigger_bhop_sequential":
                            if (!ProcessTriggerBhopSequential(prop, sb))
                                success = false;
                            break;
                        case "trigger_anti_bhop":
                            ProcessTriggerAntiBhop(prop, sb);
                            break;
                        case "trigger_anti_crouch":
                            if (!ProcessTriggerAntiCrouch(prop, sb))
                                success = false;
                            break;
                        default:
                            sb.AppendLine(prop.ToString());
                            break;
                    }
                }
                else {
                    sb.AppendLine(prop.ToString());
                }
            }
            return success;
        }

        public static void ProcessWorld(VProperty world, StringBuilder sb)
        {
            sb.AppendLine("world {");
            sb.AppendLine(Cube.SOLID);
            sb.AppendLine(world.Value.ToString().Substring(1));
        }

        public static void ProcessTriggerReset(VProperty reset, StringBuilder sb)
        {
            // Write the filter reset trigger.
            sb.AppendLine("entity {");
            sb.AppendLine("classname trigger_multiple");
            sb.AppendLine("spawnflags \"4097\"");
            sb.AppendLine("wait \"0.02\"");
            sb.AppendLine("connections {");
            sb.AppendLine($"OnStartTouch \"!activator{ESC}AddOutput{ESC}targetname default{ESC}0.02{ESC}-1\"");
            sb.AppendLine("}");
            AppendSolids(reset, sb);
            sb.AppendLine("}");
        }

        public static bool ProcessTriggerBhop(VProperty bhop, StringBuilder sb)
        {
            var propDelay = bhop.Value["delay"];
            var propTarget = bhop.Value["target"];
            var propUseLandmarkAngles = bhop.Value["UseLandmarkAngles"];

            if (bhop.Value["target"] == null) {
                WriteError("Target Destination on trigger_bhop is empty.");
                return false;
            }

            var delay = propDelay != null ? float.Parse(propDelay.ToString()) : 0.07f;
            var target = propTarget.ToString();
            var useLandmarkAngles = propUseLandmarkAngles != null ? propUseLandmarkAngles.ToString() : "0";

            var filterName = "_ex_bhop";

            // Write the teleport trigger.
            sb.AppendLine("entity {");
            sb.AppendLine("classname \"trigger_teleport\"");
            sb.AppendLine("spawnflags \"4097\"");
            sb.AppendLine($"filtername \"{filterName}\"");
            sb.AppendLine($"target \"{target}\"");
            sb.AppendLine($"UseLandmarkAngles \"{useLandmarkAngles}\"");
            AppendSolids(bhop, sb);
            sb.AppendLine("}");

            // Write the filter trigger.
            sb.AppendLine("entity {");
            sb.AppendLine("classname \"trigger_multiple\"");
            sb.AppendLine("spawnflags \"4097\"");
            sb.AppendLine("wait \"0.02\"");
            sb.AppendLine("connections {");
            sb.AppendLine($"OnStartTouch \"!activator{ESC}AddOutput{ESC}targetname {filterName}{ESC}{delay}{ESC}-1\"");
            sb.AppendLine($"OnStartTouch \"!activator{ESC}AddOutput{ESC}targetname default{ESC}{delay + 0.02}{ESC}-1\"");
            sb.AppendLine("}");
            AppendSolids(bhop, sb);
            sb.AppendLine("}");

            // Write filter.
            if (!BhopFilter) {
                BhopFilter = true;

                sb.AppendLine("entity {");
                sb.AppendLine("classname \"filter_activator_name\"");
                sb.AppendLine($"filtername \"{filterName}\"");
                sb.AppendLine("Negated \"Allow entities that match criteria\"");
                sb.AppendLine($"targetname \"{filterName}\"");
                sb.AppendLine($"origin \"{Cube.ORIGIN}\"");
                sb.AppendLine("}");
            }

            return true;
        }

        public static bool ProcessTriggerBhopSingle(VProperty bhop, StringBuilder sb)
        {
            var propDelay = bhop.Value["delay"];
            var propTarget = bhop.Value["target"];
            var propUseLandmarkAngles = bhop.Value["UseLandmarkAngles"];

            if (bhop.Value["target"] == null) {
                WriteError("Target Destination on trigger_bhop_single is empty.");
                return false;
            }

            var delay = propDelay != null ? float.Parse(propDelay.ToString()) : 0.07f;
            var target = propTarget.ToString();
            var useLandmarkAngles = propUseLandmarkAngles != null ? propUseLandmarkAngles.ToString() : "0";

            var filterName = "_ex_singlebhop" + UniqueInt;

            // Write the teleport trigger.
            sb.AppendLine("entity {");
            sb.AppendLine("classname \"trigger_teleport\"");
            sb.AppendLine("spawnflags \"4097\"");
            sb.AppendLine($"filtername \"{filterName}\"");
            sb.AppendLine($"target \"{target}\"");
            sb.AppendLine($"UseLandmarkAngles \"{useLandmarkAngles}\"");
            AppendSolids(bhop, sb);
            sb.AppendLine("}");

            // Write the filter trigger.
            sb.AppendLine("entity {");
            sb.AppendLine("classname \"trigger_multiple\"");
            sb.AppendLine("spawnflags \"4097\"");
            sb.AppendLine("wait \"0.02\"");
            sb.AppendLine("connections {");
            sb.AppendLine($"OnStartTouch \"!activator{ESC}AddOutput{ESC}targetname {filterName}{ESC}{delay}{ESC}-1\"");
            sb.AppendLine("}");
            AppendSolids(bhop, sb);
            sb.AppendLine("}");

            // Write filter.
            sb.AppendLine("entity {");
            sb.AppendLine("classname \"filter_activator_name\"");
            sb.AppendLine($"filtername \"{filterName}\"");
            sb.AppendLine("Negated \"Allow entities that match criteria\"");
            sb.AppendLine($"targetname \"{filterName}\"");
            sb.AppendLine($"origin \"{Cube.ORIGIN}\"");
            sb.AppendLine("}");

            return true;
        }

        public static bool ProcessTriggerBhopSequential(VProperty bhop, StringBuilder sb)
        {
            var propDelay = bhop.Value["delay"];
            var propTarget = bhop.Value["target"];
            var propUseLandmarkAngles = bhop.Value["UseLandmarkAngles"];
            var propIndex = bhop.Value["index"];

            if (bhop.Value["target"] == null) {
                WriteError("Target Destination on trigger_bhop_sequential is empty.");
                return false;
            }

            var delay = propDelay != null ? float.Parse(propDelay.ToString()) : 0.07f;
            var target = propTarget.ToString();
            var useLandmarkAngles = propUseLandmarkAngles != null ? propUseLandmarkAngles.ToString() : "0";
            var index = propIndex != null ? int.Parse(propIndex.ToString()) : 0;

            if (index < 0) {
                WriteError("Index on trigger_bhop_sequential is invalid.");
                return false;
            }

            var filterName = "_ex_seqbhop" + index;
            var filterNamePrev = (index == 1) ? "default" : "_ex_seqbhop" + (index - 1);

            // Write the teleport trigger.
            sb.AppendLine("entity {");
            sb.AppendLine("classname \"trigger_teleport\"");
            sb.AppendLine("spawnflags \"4097\"");
            sb.AppendLine($"filtername \"{filterName}\"");
            sb.AppendLine($"target \"{target}\"");
            sb.AppendLine($"UseLandmarkAngles \"{useLandmarkAngles}\"");
            AppendSolids(bhop, sb);
            sb.AppendLine("}");

            // Write the filter trigger.
            sb.AppendLine("entity {");
            sb.AppendLine("classname \"trigger_multiple\"");
            sb.AppendLine("spawnflags \"4097\"");
            sb.AppendLine("wait \"0.02\"");
            sb.AppendLine("connections {");
            sb.AppendLine($"OnStartTouch \"!activator{ESC}AddOutput{ESC}targetname {filterName}{ESC}{delay}{ESC}-1\"");
            sb.AppendLine("}");
            AppendSolids(bhop, sb);
            sb.AppendLine("}");

            // Write filter.
            if (!SequentialFilters.Contains(index)) {
                SequentialFilters.Add(index);

                sb.AppendLine("entity {");
                sb.AppendLine("classname \"filter_activator_name\"");
                sb.AppendLine($"filtername \"{filterNamePrev}\"");
                sb.AppendLine("Negated \"1\"");
                sb.AppendLine($"targetname \"{filterName}\"");
                sb.AppendLine($"origin \"{Cube.ORIGIN}\"");
                sb.AppendLine("}");
            }

            return true;
        }

        public static void ProcessTriggerAntiBhop(VProperty antibhop, StringBuilder sb)
        {
            sb.AppendLine("entity {");
            sb.AppendLine("classname \"trigger_multiple\"");
            sb.AppendLine("spawnflags \"4097\"");
            sb.AppendLine("wait \"0.02\"");
            sb.AppendLine("connections {");
            sb.AppendLine($"OnStartTouch \"!activator{ESC}RunScriptCode{ESC}local v=self.GetVelocity();if(v.z>0.01){{v.z=-v.z*0.25;self.SetVelocity(v);}}{ESC}0.01{ESC}-1\"");
            sb.AppendLine($"OnStartTouch \"!activator{ESC}RunScriptCode{ESC}local v=self.GetVelocity();if(v.z>0.01){{v.z=-v.z*0.50;self.SetVelocity(v);}}{ESC}0.05{ESC}-1\"");
            sb.AppendLine($"OnStartTouch \"!activator{ESC}AddOutput{ESC}gravity 1{ESC}0.2{ESC}-1\"");
            sb.AppendLine($"OnStartTouch \"!activator{ESC}AddOutput{ESC}gravity 40{ESC}0.1{ESC}-1\"");
            sb.AppendLine("}");
            AppendSolids(antibhop, sb);
            sb.AppendLine("}");
        }

        public static bool ProcessTriggerAntiCrouch(VProperty anticrouch, StringBuilder sb)
        {
            var propTarget = anticrouch.Value["target"];
            var propUseLandmarkAngles = anticrouch.Value["UseLandmarkAngles"];

            if (anticrouch.Value["target"] == null) {
                WriteError("Target Destination on trigger_anti_crouch is empty.");
                return false;
            }

            var target = propTarget.ToString();
            var useLandmarkAngles = propUseLandmarkAngles != null ? int.Parse(propUseLandmarkAngles.ToString()) != 0 : false;

            if (!TeleportDestinations.TryGetValue(target, out var tpDest)) {
                WriteError($"Target Destination '{target}' on trigger_anti_crouch doesn't exist.");
                return false;
            }

            var px = tpDest.px;
            var py = tpDest.py;
            var pz = tpDest.pz;
            var rx = tpDest.rx;
            var ry = tpDest.ry;
            var rz = tpDest.rz;

            // Write the trigger.
            sb.AppendLine("entity {");
            sb.AppendLine("classname \"trigger_multiple\"");
            sb.AppendLine("spawnflags \"4097\"");
            sb.AppendLine("wait \"0.02\"");
            sb.AppendLine("connections {");
            if (useLandmarkAngles) {
                sb.AppendLine($"OnTrigger \"!activator{ESC}RunScriptCode{ESC}if(self.GetBoundingMaxs().z<70.0){{local v=self.GetVelocity();v.z=0;self.SetVelocity(v);self.SetOrigin(Vector({px},{py},{pz}));self.SetAngles({rx},{ry},{rz});}}{ESC}0.03{ESC}-1\"");
            }
            else {
                sb.AppendLine($"OnTrigger \"!activator{ESC}RunScriptCode{ESC}if(self.GetBoundingMaxs().z<70.0){{local v=self.GetVelocity();v.z=0;self.SetVelocity(v);self.SetOrigin(Vector({px},{py},{pz}));}}{ESC}0.03{ESC}-1\"");
            }
            sb.AppendLine("}");
            AppendSolids(anticrouch, sb);
            sb.AppendLine("}");

            return true;
        }
    }
}
