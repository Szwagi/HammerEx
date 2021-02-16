# HammerEx
Valve Hammer Editor addon that adds convinience entities for KZ mappers.
Written in a couple of hours as a test, if you're looking for quality code this is not the place, but it works.

## Features
- `trigger_reset`  Reset trigger for trigger_bhop_single etc.
- `trigger_bhop`  Forces a bhop, can bhop on the same block as many times as you want
- `trigger_bhop_single`  Forces a bhop, can only bhop on a block once, jumping on another single bhop resets it
- `trigger_bhop_sequential`  Forces a bhop, can only bhop on a block once, in the correct order
- `trigger_anti_bhop`  Blocks bhopping on a block
- `trigger_anti_crouch`  Blocks crouching

## How to install
1. [Download HammerEx from releases](https://github.com/Szwagi/HammerEx/releases).
2. Unzip the contents into /bin/ inside your CS:GO directory. *Backup CmdSeq.wc*
3. Download and open the [Example Map](https://raw.githubusercontent.com/Szwagi/HammerEx/main/kz_hammerex_example.vmf). *Right-click and Save As...*
4. Go to Expert/Advanced compile settings and choose one of the HammerEx configurations.
