var ffi = require('ffi');
var ref = require('ref');

var bytePtr = ref.refType(ref.types.byte);

var rust_wrapper = ffi.Library('rustwrapper', 
{
    'write_image_byte_array_to_file' : ['int', [bytePtr, 'int', 'int', 'int'] ]
});

window.bridge = {};
window.bridge.saveTexture = function (pixel_array, array_length, width, height)
{
    var buf = Buffer.alloc(array_length);
    var t = 0;
    for (var i = 0; i < height; i++)
    {
        for (var j = width - 1; j >= 0; j--)
        {
            for (var k = 0; k < 3; k++)
            {
                var index = (i * width * 3.0 + j * 3.0) + k;
                buf[t] = pixel_array[index];
                t++;
            }
        }
    }
    rust_wrapper.write_image_byte_array_to_file(buf, array_length, width, height);
};