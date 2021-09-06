(function ($) {
    $.validator.addMethod("notequalto", function (value, element, params) {
        if (!this.optional(element)) {
            var otherProp = $('#' + params)
            return (otherProp.val() != value);
        }
        return true;
    });
    $.validator.unobtrusive.adapters.addSingleVal("notequalto", "otherproperty");

    $.validator.addMethod("requiredif", function (value, element, params) {
        if ($(element).val() != '') return true

        var $other = $('#' + params.other);

        var otherVal = ($other.attr('type').toUpperCase() == "CHECKBOX") ?
            ($other.is(":checked") ? "true" : "false") : $other.val();

        var result = params.comp == 'isequalto' ? (otherVal != params.value)
            : (otherVal == params.value);

        return result;
        //return true; passes validation for Validation code so no message shown to user
    });

    $.validator.unobtrusive.adapters.add("requiredif", ["other", "comp", "value"],
        function (options) {
            options.rules['requiredif'] = {
                other: options.params.other,
                comp: options.params.comp,
                value: options.params.value
            };
            options.messages['requiredif'] = options.message;
        }
    );

    $.validator.addMethod("checkcountry", function (value, element, params) {
        if (!this.optional(element)) {
            return params.AllowCountry.toUpperCase().split(',').includes(value.toUpperCase().trim());
        }
        return false;
    });

    $.validator.unobtrusive.adapters.add('checkcountry',
        ['AllowCountry'],
        function (options) {
            options.rules['checkcountry'] = {
                AllowCountry: options.params.AllowCountry
            };
            options.messages['checkcountry'] = options.message;
        });

}(jQuery));