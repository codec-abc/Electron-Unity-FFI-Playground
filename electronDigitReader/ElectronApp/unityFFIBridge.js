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

    var float_array_to_send_texture_to_cntk = Buffer.alloc(width * height * 4);
    var byte_array_to_save_texture_with_rust_dll = Buffer.alloc(width * height * 3);
    var float_array = [];

    var float_array_as_string = "";
    var rgb_texture_byte_index = 0;
    var mono_texture_float_index = 0;
    for (var i = 0; i < width; i++)
    {
        var current_texture_line_as_float = ""
        for (var j = height - 1; j >= 0; j--)
        {
            var sum = 0;
            for (var k = 0; k < 3; k++)
            {
                var index = (i * width * 3 + j * 3) + k;
                sum += pixel_array[index];
                byte_array_to_save_texture_with_rust_dll[rgb_texture_byte_index] = pixel_array[index];
                rgb_texture_byte_index++;
            }
            var pixel_value = 255.0 - (sum / 3.0);
            float_array_to_send_texture_to_cntk.writeFloatLE(pixel_value, mono_texture_float_index * 4);
            float_array.push(float_array_to_send_texture_to_cntk.readFloatLE(mono_texture_float_index * 4));
            current_texture_line_as_float += float_array[mono_texture_float_index] + " " ;
            mono_texture_float_index++;
        }
        float_array_as_string += current_texture_line_as_float + "\r\n";
    }

    var probas = Buffer.alloc(10 * 4);
    var result = cntk_wrapper.analyze_digits(float_array_to_send_texture_to_cntk, probas);

    var probas_as_float = [];
    var max_proba = -999;
    var max_proba_digit = -1;
    var stat = [];

    for (var i = 0; i < 10; i++)
    {
        var obj = {};
        var proba = probas.readFloatLE(i * 4);
        probas_as_float.push(proba);

        if (proba > max_proba)
        {
            max_proba = proba;
            max_proba_digit = i;
        }
        
        obj.digit = i;
        obj.proba = proba;
        stat.push(obj);
    }

    //console.log(float_array_as_string);
    //console.log(stat);
    
    alert("most probable digit " + max_proba_digit);

    // rust_wrapper.write_rgb_texture_byte_array_to_file(byte_array_to_save_texture_with_rust_dll, width * height * 3, width, height);
};