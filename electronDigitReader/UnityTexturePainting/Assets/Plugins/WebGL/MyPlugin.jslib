var MyPlugin = {
    SendRGBTexture: function(array, size, width, height)
    {
        var myArray  = new Array(size);
        for(var i=0;i<size;i++)
        {
            var myFloat = (HEAPF32[(array>>2)+i]);
            myArray[i] = myFloat;
        }
        window.bridge.saveRGBTexture(myArray, size, width, height);
    }
};

mergeInto(LibraryManager.library, MyPlugin);