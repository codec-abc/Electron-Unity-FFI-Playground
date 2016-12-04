var ffi = require('ffi');
var ref = require('ref');

var bytePtr = ref.refType(ref.types.byte);
var floatPtr = ref.refType(ref.types.float);

var rust_wrapper = ffi.Library('rustwrapper', 
{
    'write_rgb_texture_byte_array_to_file' : ['int', [bytePtr, 'int', 'int', 'int'] ]
});

var cntk_wrapper = ffi.Library('CPPEvalClientDll', 
{
    'analyze_digits' : ['int', [floatPtr, floatPtr] ]
});

window.bridge = {};
window.bridge.HandleRGBTexture = function (pixel_array, width, height)
{
    var isOk = (width === height && height === 28);
    if (!isOk)
    {
        var msg = "The cntk custom dlls except image of width and height of 28 and the image given does not fit those requirements.";
        alert(msg)
        throw msg;
    }

    var buf = Buffer.alloc(width * height * 4);
    var buf2 = Buffer.alloc(width * height * 3);
    var float_array = [];

    var z = 0;
    var t = 0;
    for (var i = 0; i < width; i++)
    {
        for (var j = height - 1; j >= 0; j--)
        {
            var sum = 0;
            for (var k = 0; k < 3; k++)
            {
                var index = (i * width * 3 + j * 3) + k;
                sum += pixel_array[index];
                buf2[z] = pixel_array[index];
                z++;
            }
            var pixel_value = 255.0 -(sum / 3.0);
            buf.writeFloatLE(pixel_value, t * 4);
            float_array.push(pixel_value);
            t++;
        }
    }

    var probas = Buffer.alloc(10 * 4);
    var result = cntk_wrapper.analyze_digits(buf, probas);

    var probas_as_float = [];
    var max_proba = -999;
    var max_proba_digit = -1;

    for (var i = 0; i < 10; i++)
    {
        var proba = probas.readFloatLE(i * 4);
        probas_as_float.push(proba);

        if (proba > max_proba)
        {
            max_proba = proba;
            max_proba_digit = i;
        }
    }

    /*
    var total = 0;
    var new_msg = "";
    for (var i = 0; i < width; i++)
    {
        var mid_string = ""
        for (var j = 0; j < width; j++)
        {
            mid_string += float_array[total] + " " ;
            total++;
        }
        new_msg += mid_string + "\r\n";
    }
    console.log(new_msg);
    */
    
    alert("most probable digit " + max_proba_digit);

    // rust_wrapper.write_rgb_texture_byte_array_to_file(buf2, width * height * 3, width, height);
};