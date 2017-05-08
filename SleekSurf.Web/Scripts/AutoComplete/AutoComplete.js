function AddSlash(match) {
    return "\\" + match;
}

$(document).ready(function () {

    if ($(".autoComplete").length > 0) {
        var autoCompleteBox = $(".autoComplete");

        $(".autoComplete").focus(function () {
            if (autoCompleteBox.val() == this.title) {
                autoCompleteBox.removeClass("waterMark");
                autoCompleteBox.val("");
            }
        });

        autoCompleteBox.blur(function () {
            if (autoCompleteBox.val() == "" || autoCompleteBox.val() == this.title) {
                autoCompleteBox.addClass("waterMark");
                autoCompleteBox.val(this.title);
            }
        });

        autoCompleteBox.blur();

        $(".autoComplete").autocomplete({
            source: function (request, response) {
                var re = /['\\"]/g;
                var searchString = request.term;
                searchString = searchString.replace(re, function (m) { return AddSlash(m) });
                $.ajax({
                    url: '../../AutoCompleteLocalService.asmx/GetMatchingKeyword',
                    data: "{ 'prefixText': '" + searchString + "' , 'nRecordSet':'10'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        if (data.d != null) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item
                                }
                            }))
                        }
                        else
                            response($.map(0, function (item) {
                                return {
                                    value: item
                                }
                            }))

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            minLength: 1
        });
    }
});