extern crate image;
extern crate libc;

use std::slice;

#[no_mangle]
pub extern fn write_image_byte_array_to_file
(
    ptr: *const libc::uint8_t, 
    len : libc::size_t, 
    width : libc:: uint32_t, 
    height : libc::uint32_t
) -> i32
{
    let slice = unsafe {
        assert!(!ptr.is_null());
        slice::from_raw_parts(ptr, len)
    };

    let result = image::save_buffer(
        "C:\\Users\\codec\\Desktop\\test2.png",
        &slice,
        width,
        height,
        image::ColorType::RGB(8)
    );
    match result {
        Ok(_) => 1,
        Err(_) => 0
    }
}
