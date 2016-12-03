# Electron-Unity-FFI-Playground

This project is a fun little project to mix different technologies and framework. It mixes Unity, WebGL, Electron, Nodejs, Rust and CNTK. The main goal is to draw a digit in a WebGL canvas and to identify the digit using a machine learning algorithm. For that we use Unity to create a simple drawing app. [I used this project as a base to do the painting.](https://www.assetstore.unity3d.com/en/#!/content/33506)

Then I use the ability that this is an Electron application to do some fun stuff with Node. I use the `ffi` and `ref` package to get the texture content and forward it to native librairies. I can either send it to a dll build with Rust that use the `image` crate to save it as a PNG in the home folder or I can send it to a CNTK dll that try to identify the digit and send back the result to the app.

## How to build 
- Run ElectronApp/MSMpiSetup.exe once to install CNTK dependencies
- Build the unity project for WebGL and place the output in the ElectronApp directory
- Build the Rust wrapper dll and place it in the ElectronApp directory
- cd ElectronApp
- npm start

## Save texture as PNG

The app can use a dll build using Rust to save the texture to a file. For that follow the build instructions and open the file ElectronApp\unityBridge.js and uncomment the following line
```js
//rust_wrapper.write_rgb_texture_byte_array_to_file(buf2, width * height * 3, width, height);
```

## CNTK custom dll

[If you want to build CNTK grab the CPU release here.](https://github.com/Microsoft/CNTK/releases/tag/v2.0.beta5.0)

Then open the solution at cntk\Examples\Evaluation\EvalClients.sln
Change the type of the CPPEvalClient from executable to Dynamic Library and use the source file. Don't forget to grab the needed dlls in the cntk directory.
