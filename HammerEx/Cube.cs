using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammerEx
{
    public static class Cube
    {
        public const string ORIGIN = @"16288 16288 16288";

        public const string SOLID = @"
            solid {
             side {
               plane ""(16248 16328 16328) (16328 16328 16328) (16328 16248 16328)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16248 16328 16328) (16248 16248 16328) (16248 16248 16248)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16328 16248) (16328 16248 16248) (16328 16248 16328)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16328 16328) (16248 16328 16328) (16248 16328 16248)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16248 16248) (16248 16248 16248) (16248 16248 16328)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16320 16256 16320) (16320 16320 16320) (16256 16320 16320)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
            }
            solid {
             side {
               plane ""(16248 16248 16248) (16328 16248 16248) (16328 16328 16248)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16248 16328 16328) (16248 16248 16328) (16248 16248 16248)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16328 16248) (16328 16248 16248) (16328 16248 16328)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16328 16328) (16248 16328 16328) (16248 16328 16248)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16248 16248) (16248 16248 16248) (16248 16248 16328)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16320 16320 16256) (16320 16256 16256) (16256 16256 16256)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
            }
            solid {
             side {
               plane ""(16248 16328 16328) (16248 16248 16328) (16248 16248 16248)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16328 16328) (16248 16328 16328) (16248 16328 16248)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16248 16248) (16248 16248 16248) (16248 16248 16328)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16256 16320 16320) (16320 16320 16320) (16320 16256 16320)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16256 16256 16256) (16320 16256 16256) (16320 16320 16256)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16256 16256 16256) (16256 16256 16320) (16256 16320 16320)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
            }
            solid {
             side {
               plane ""(16328 16328 16248) (16328 16248 16248) (16328 16248 16328)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16328 16328) (16248 16328 16328) (16248 16328 16248)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16328 16248 16248) (16248 16248 16248) (16248 16248 16328)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16256 16320 16320) (16320 16320 16320) (16320 16256 16320)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16256 16256 16256) (16320 16256 16256) (16320 16320 16256)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16320 16256 16320) (16320 16256 16256) (16320 16320 16256)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
            }
            solid {
             side {
               plane ""(16328 16328 16328) (16248 16328 16328) (16248 16328 16248)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16256 16320 16320) (16320 16320 16320) (16320 16256 16320)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16256 16256 16256) (16320 16256 16256) (16320 16320 16256)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16256 16320 16320) (16256 16256 16320) (16256 16256 16256)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16320 16320 16256) (16320 16256 16256) (16320 16256 16320)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16256 16320 16256) (16256 16320 16320) (16320 16320 16320)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
            }
            solid {
             side {
               plane ""(16328 16248 16248) (16248 16248 16248) (16248 16248 16328)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16256 16320 16320) (16320 16320 16320) (16320 16256 16320)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16256 16256 16256) (16320 16256 16256) (16320 16320 16256)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 -1 0 0] 0.25""
             }
             side {
               plane ""(16256 16320 16320) (16256 16256 16320) (16256 16256 16256)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16320 16320 16256) (16320 16256 16256) (16320 16256 16320)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[0 1 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
             side {
               plane ""(16256 16256 16320) (16256 16256 16256) (16320 16256 16256)""
               material ""TOOLS/TOOLSSKYBOX""
               uaxis ""[1 0 0 0] 0.25""
               vaxis ""[0 0 -1 0] 0.25""
             }
            }
            ";
    }
}
