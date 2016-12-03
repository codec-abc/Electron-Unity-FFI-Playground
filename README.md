# Electron-Unity-FFI-Playground

## How to build 
- Run ElectronApp/MSMpiSetup.exe once to install CNTK dependencies
- Build the unity project for WebGL and place the output in the ElectronApp directory
- Build the Rust wrapper dll and place it in the ElectronApp directory
- cd ElectronApp
- npm start

## CNTK custom dll

[If you want to build CNTK grab the CPU release here](https://github.com/Microsoft/CNTK/releases/tag/v2.0.beta5.0)
Then open the solution at cntk\Examples\Evaluation\EvalClients.sln
Change the type of the CPPEvalClient from executable to Dynamic Library and use the source file. Don't forget to grab the needed dlls in the cntk directory.
