//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
// CPPEvalClient.cpp : Sample application using the evaluation interface from C++
//
#include <sys/stat.h>
#include "Eval.h"
#ifdef _WIN32
#include "Windows.h"
#endif

using namespace Microsoft::MSR::CNTK;

// Used for retrieving the model appropriate for the element type (float / double)
template<typename ElemType>
using GetEvalProc = void(*)(IEvaluateModel<ElemType>**);

typedef std::pair<std::wstring, std::vector<float>*> MapEntry;
typedef std::map<std::wstring, std::vector<float>*> Layer;

int main(float* image, float* result)
{
    int ret;
    IEvaluateModel<float> *model;
    const std::string modelFilePath = "02_OneConv";

    try
    {
        struct stat statBuf;
        if (stat(modelFilePath.c_str(), &statBuf) != 0)
        {
            fprintf(stderr, "Error: The model %s does not exist. Please follow instructions in README.md in <CNTK>/Examples/Image/GettingStarted to create the model.\n", modelFilePath.c_str());
            return(1);
        }

        GetEvalF(&model);

        // Load model with desired outputs
        std::string networkConfiguration;
        // Uncomment the following line to re-define the outputs (include h1.z AND the output ol.z)
        // When specifying outputNodeNames in the configuration, it will REPLACE the list of output nodes 
        // with the ones specified.
        //networkConfiguration += "outputNodeNames=\"h1.z:ol.z\"\n";
        networkConfiguration += "modelPath=\"" + modelFilePath + "\"";
        model->CreateNetwork(networkConfiguration);

        // get the model's layers dimensions
        std::map<std::wstring, size_t> inDims;
        std::map<std::wstring, size_t> outDims;
        model->GetNodeDimensions(inDims, NodeGroup::nodeInput);
        model->GetNodeDimensions(outDims, NodeGroup::nodeOutput);

        // Generate dummy input values in the appropriate structure and size
        auto inputLayerName = inDims.begin()->first;
        std::vector<float> inputs;

        
        for (int i = 0; i < inDims[inputLayerName]; i++)
        {
            inputs.push_back(image[i]);
        }

        // Allocate the output values layer
        std::vector<float> outputs;

        // Setup the maps for inputs and output
        Layer inputLayer;
        inputLayer.insert(MapEntry(inputLayerName, &inputs));
        Layer outputLayer;
        auto outputLayerName = outDims.begin()->first;
        outputLayer.insert(MapEntry(outputLayerName, &outputs));

        // We can call the evaluate method and get back the results (single layer)...
        model->Evaluate(inputLayer, outputLayer);

        // Output the results
        int index = 0;
        for (auto& value : outputs)
        {
            result[index] = value;
            index++;
        }

        ret = 0;
    }
    catch (const std::exception& err)
    {
        fprintf(stderr, "Evaluation failed. EXCEPTION occurred: %s\n", err.what());
        ret = 2;
    }
    catch (...)
    {
        fprintf(stderr, "Evaluation failed. Unknown ERROR occurred.\n");
        ret = 3;
    }

    fflush(stderr);
    return ret;
}

extern "C" __declspec(dllexport) int __cdecl analyze_digits(float* image, float* result)
{
    return main(image, result);
}
