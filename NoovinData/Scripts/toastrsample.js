$(function () {
    $('#showtoast').click(function () {
        toastr.options =
          {
              "debug": false,
              "positionClass": $("#toastrPositionGroup input:radio:checked").val(),
              "onclick": null,
              "fadeInLeft ": 300,
              "fadeOut": 100,
              "timeOut": 5000,
              "extendedTimeOut": 3000
          }

        var d = "با موفقیت انجام شد";
        toastr[$("#toastrTypeGroup input:radio:checked").val()](d,": عملیات شما ");
    });


    $('#showtoasterror').click(function () {
        toastr.options =
          {
              "debug": false,
              "positionClass": $("#toastrPositionGroup input:radio:checked").val(),
              "onclick": null,
              "fadeInLeft ": 300,
              "fadeOut": 100,
              "timeOut": 5000,
              "extendedTimeOut": 3000
          }

        var d = "با موفقیت انجام نشد";
        toastr[$("#toastrTypeGroup input:radio:checked").val()](d, ": عملیات شما ");
    });
    $('#showtoastwarning').click(function () {
        toastr.options =
          {
              "debug": false,
              "positionClass": $("#toastrPositionGroup input:radio:checked").val(),
              "onclick": null,
              "fadeInLeft ": 300,
              "fadeOut": 100,
              "timeOut": 5000,
              "extendedTimeOut": 3000
          }

        var d = "با موفقیت انجام شد";
        toastr[$("#toastrTypeGroup input:radio:checked").val()](d, ": عملیات شما ");
    });
});

