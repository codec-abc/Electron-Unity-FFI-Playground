var MyPlugin = {
    SendRGBTexture: function(array, size, width, height)
    {
        var texturePixelsArray  = new Array(size);
        for(var i=0;i<size;i++)
        {
            var myFloat = (HEAPF32[(array>>2)+i]);
            texturePixelsArray[i] = myFloat;
        }
        window.bridge.HandleRGBTexture(texturePixelsArray, width, height);
    }
};

mergeInto(LibraryManager.library, MyPlugin);