Dropzone.autoDiscover = false;

function collapseDivUpload(menuItemId, yelpBusinessId, divUpload, divDropzone) {
    let $divUpload = $('#' + divUpload);
    if ($divUpload.hasClass("d-none")) {
        let instance = $.grep(Dropzone.instances, function (o) { return o.element.id == divDropzone })[0];
        if (instance == null) {
            $('#' + divDropzone).dropzone({
                url: "/Home/UploadImage",
                autoProcessQueue: true,
                paramName: "file",
                maxFiles: 5,
                maxFilesize: 10,    //mb
                dictMaxFilesExceeded: "Please upload only 5 images at a time",
                acceptedFiles: "image/*",
                dictInvalidFileType: "Please upload only image files",
                params: {
                    "menuItemId": menuItemId,
                    "yelpBusinessId": yelpBusinessId
                },
                success: function (file, response) {
                    let lightSlider = LightSliderInstances.dictInstances['lightSlider-' + yelpBusinessId]
                    if (lightSlider != null) {
                        let fileRelativeUrl = response.fileRelativeUrl;
                        let fileRelativeUrlThumb = response.fileRelativeUrlThumb;

                        let li = '<li class="lslide" data-thumb="' + fileRelativeUrlThumb + '" data-src="' + fileRelativeUrl + '"><img src="' + fileRelativeUrl + '" height="75" /></li>';
                        $("#lightSlider-" + yelpBusinessId).prepend(li);
                        
                        lightSlider.data('lightGallery').destroy(true);
                        lightSlider.lightGallery();
                        lightSlider.refresh();
                        lightSlider.goToSlide(0);
                    }
                    this.removeFile(file);
                },
                error: function (file, response) {
                    toastError(file.name + ': ' + (response.errorMessage || response));
                },
                complete: function (file) {
                    this.removeFile(file);
                }
            });
        }
        $divUpload.removeClass("d-none");
    }
    else {
        $divUpload.addClass("d-none");
    }
}

let LightSliderInstances = (function () {
    let dictInstances = {};
    return {
        dictInstances: dictInstances
    }
})();

let FingerprintHashCookie = (function () {
    const COOKIE_NAME = "fingerprintHash";

    function _getCookie(name) {
        let value = "; " + document.cookie;
        let parts = value.split("; " + name + "=");
        if (parts.length == 2) return parts.pop().split(";").shift();
    }

    function _setCookie(name, value, days) {
        let expires = "";
        if (days) {
            let date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + (value || "") + expires + "; path=/";
    }

    function getCookie() {
        return _getCookie(COOKIE_NAME);
    }

    function setCookie(value) {
        _setCookie(COOKIE_NAME, value, null);
    }

    return {
        getCookie: getCookie,
        setCookie: setCookie,
    }

})();

function selectMenuItem(sender, menuItemId, yelpBusinessId) {
    debugger;
    let $row = $(sender);
    if (!$row.hasClass("menuItemSelected")) {
        $row.parent().find("tr").removeClass("menuItemSelected");
        $row.addClass("menuItemSelected");

        getMenuItemImagesAndComments(menuItemId, yelpBusinessId);
    }
}

function getMenuItemImagesAndComments(menuItemId, yelpBusinessId) {
    $.ajax({
        url: "Home/GetMenuItemImagesAndComments",
        type: "POST",
        data: {
            menuItemId: menuItemId,
            yelpBusinessId: yelpBusinessId
        },
        success: function (result) {
            $("#divMenuItemInfo-" + yelpBusinessId).html(result);
        }
    });
}

function toggleCommentSentiment(sender) {
    let $this = $(sender);
    if ($this.attr("id") == "commentHeart") {
        $this.parent().find("#commentThumbsDown").removeClass("selectedIcon");
    }
    else if ($this.attr("id") == "commentThumbsDown") {
        $this.parent().find("#commentHeart").removeClass("selectedIcon");
    }

    $this.toggleClass("selectedIcon");
}

function addComment(menuItemId, yelpBusinessId) {
    let commentSentiment;
    let $divMenuIteminfo = $("#divMenuItemInfo-" + yelpBusinessId);
    let $txtComment = $divMenuIteminfo.find("#txtComment");

    if ($txtComment.val() == '') {
        alertTooltip($txtComment, "Field is required");
        return false;
    }

    if ($divMenuIteminfo.find("#commentHeart").hasClass("selectedIcon"))
        commentSentiment = 1; //Sentiment.Positive
    else if ($divMenuIteminfo.find("#commentThumbsDown").hasClass("selectedIcon"))
        commentSentiment = 0; //Sentiment.Negative
    else
        commentSentiment = -1; //Sentiment.Unknown

    $.ajax({
        url: "Home/AddComment",
        type: "POST",
        data: {
            text: $txtComment.val(),
            commentSentiment: commentSentiment,
            menuItemId: menuItemId,
            yelpBusinessId: yelpBusinessId
        },
        success: function (result) {
            $divMenuIteminfo.html(result);
            toastSuccess('Comment added');
        },
        error: function (xhr, status, error) {
            toastError(xhr.responseJSON.errorMessage || error);
        }
    });

    return false;
}

function addMenuItem(yelpBusinessId, yelpBusinessAlias) {
    let $divBusinessMenu = $("#divBusinessMenu-" + yelpBusinessId);
    let $divBusinessMenuWrapper = $("#divBusinessMenuWrapper-" + yelpBusinessId);
    
    let $txtMenuItem = $divBusinessMenuWrapper.find("#txtMenuItem");
    if ($txtMenuItem.val() == '') {
        alertTooltip($txtMenuItem, "Field is required");
        return false;
    }

    $.ajax({
        url: "Home/AddMenuItem",
        type: "POST",
        data: {
            txtMenuItem: $txtMenuItem.val(),
            yelpBusinessId: yelpBusinessId,
            yelpBusinessAlias: yelpBusinessAlias
        },
        success: function (result) {
            $divBusinessMenu.html(result);

            let $row = $divBusinessMenu.find("tr.menuItemSelected");
            let menuItemId = $row.attr("data-id");
            getMenuItemImagesAndComments(menuItemId, yelpBusinessId);

            //let rowIndex = $row[0].rowIndex;
            //if (rowIndex > 5) {
            //    $divBusinessMenu.find("tr").eq(rowIndex - 5)[0].scrollIntoView();
            //}
            toastSuccess('Menu item added');
        },
        error: function (xhr, status, error) {
            toastError(xhr.responseJSON.errorMessage || error);
        }
    });

    return false;
}

function addLike(likeAction, menuItemId, yelpBusinessId) {
    $.ajax({
        url: "Home/AddLike",
        type: "POST",
        data: {
            likeAction: likeAction,
            menuItemId: menuItemId,
            yelpBusinessId: yelpBusinessId
        },
        success: function (result) {
            $("#divBusinessMenu-" + yelpBusinessId).html(result);
            toastSuccess('Vote recorded');
        },
        error: function (xhr, status, error) {
            toastError(xhr.errorMessage || error);
        }
    });
}

function toastSuccess(msg) {
    $.toast({
        text: msg,
        icon: 'success',
        position: 'top-center',
        hideAfter: 3000
    });
}
function toastError(msg) {
    $.toast({
        text: msg,
        icon: 'error',
        position: 'top-center',
        hideAfter: 3000
    });
}

function alertTooltip($el, msg) {
    $el.tooltip("dispose")
        .tooltip({ "title": msg, "animation": true })
        .tooltip("show");

    //remove after 3 sec
    setTimeout(function () {
        $el.tooltip("dispose");
    }, 3000)
}

function isEmpty(value) {
    return (value == null || value === '');
}

