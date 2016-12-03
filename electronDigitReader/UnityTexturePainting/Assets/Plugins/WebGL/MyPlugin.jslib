var MyPlugin = {
    Hello: function()
    {
        window.alert("Hello, world!");
    },
    HelloString: function(str)
    {
        window.alert(Pointer_stringify(str));
    },
    PrintFloatArray: function(array, size)
    {
        for(var i=0;i<size;i++)
            console.log(HEAPF32[(array>>2)+i]);
    },
    GetSomeFloatArray: function(array, size, width, height)
    {
        var myArray  = new Array(size);
        for(var i=0;i<size;i++)
        {
            var myFloat = (HEAPF32[(array>>2)+i]);
            myArray[i] = myFloat;
        }
        window.bridge.saveTexture(myArray, size, width, height);
    },
    AddNumbers: function(x,y)
    {
        return x + y;
    },
    StringReturnValueFunction: function()
    {
        var returnStr = "bla";
        var buffer = _malloc(lengthBytesUTF8(returnStr) + 1);
        writeStringToMemory(returnStr, buffer);
        return buffer;
    },
    BindWebGLTexture: function(texture)
    {
        GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    }
};

mergeInto(LibraryManager.library, MyPlugin);