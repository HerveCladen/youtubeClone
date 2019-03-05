var defaultRangeValidator = $.validator.methods.range;
$.validator.methods.range = function (value, element, param) {
    if (element.type === 'checkbox') {
        return element.checked;
    } else {
        return defaultRangeValidator.call(this, value, element, param);
    }
}