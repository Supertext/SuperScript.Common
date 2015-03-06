var PageManager = function (win, doc, $) {

    var self = this,
        elmntContent = doc.getElementById("content"),
        location = doc.location,
        projectName = location.href.split("/")[3],
        issueUrl = "https://github.com/Supertext/" + projectName + "/issues/new",
        urlSegments = location.pathname.split("/"),
        removeLastSegment = urlSegments.pop(),
        urlDirectory = location.protocol + "//" + location.host + urlSegments.join("/") + "/documentation/",
        escapeRegExp = function(string) {
            return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
        },
        replaceAll = function(string, find, replace) {
            return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
        };

    configureRouting = function() {
        Routing.map("#!/:contentName(/:anchor)")
            .before(function() {
                elmntContent.innerHTML = "<img src=\"https://assets-cdn.github.com/images/spinners/octocat-spinner-32.gif\" alt=\"loading...\" class=\"center-block\" />";
                $("li.active").removeClass("active");
            })
            .to(function() {
                var specificPath = replaceAll(this.params.contentName.value, ".", "/") + ".html",
                    anchor = this.params.anchor.value,
                    loadUrl = urlDirectory + specificPath,
                    elmntLink = $("a[href='#!/" + this.params.contentName.value + "']");

                $.ajax({
                    dataType: "html",
                    type: "GET",
                    url: loadUrl,
                    success: function (data) {

                        elmntContent.innerHTML = data;

                        SyntaxHighlighter.highlight();

                        if (typeof (anchor) !== "undefined") {
                            var elmntAnchor = doc.getElementById(anchor);
                            if (elmntAnchor !== null) {
                                elmntAnchor.scrollIntoView(true);
                            }
                        }
                    },
                    error: function(xmlHttpRequest, textStatus, errorThrown) {
                        elmntContent.innerHTML = "<p>Sorry, it looks like an error occurred!</p><p>How about letting us know by creating an <a href=\"" + issueUrl + "\" target=\"_blank\">issue</a>?</p>";
                    }
                });

                elmntContent.className = this.params.contentName.value;

                if (elmntLink.length > 0) {
                    var elmntLi = elmntLink.parent("li");
                    elmntLi.addClass("active");
                    elmntLi.parents("li.dropdown").addClass("active");
                }
            });
    };

    return {
        "Init": function () {
            configureRouting();
        }
    };
}(window, window.document, jQuery);

$(function () {
    PageManager.Init();
    Routing.root("#!/index");
    Routing.listen();

    SyntaxHighlighter.defaults["gutter"] = false;
    SyntaxHighlighter.defaults["toolbar"] = false;
    SyntaxHighlighter.all();
});
