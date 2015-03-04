var PageManager = function (win, doc, Sizzle) {

    var self = this,
		elmntContent = doc.getElementById("content"),
		location = doc.location,
		projectName = location.href.split("/")[3],
		issueUrl = "https://github.com/Supertext/" + projectName + "/issues/new",
		urlSegments = location.pathname.split("/"),
		removeLastSegment = urlSegments.pop(),
		urlDirectory = location.protocol + "//" + location.host + urlSegments.join("/") + "/documentation/",
		escapeRegExp = function (string) {
		    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
		},
		replaceAll = function (string, find, replace) {
		    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
		},
		createXhr = function () {
		    var xhr;
		    if (window.ActiveXObject) {
		        try {
		            xhr = new ActiveXObject("Microsoft.XMLHTTP");
		        }
		        catch (e) {
		            alert(e.message);
		            xhr = null;
		        }
		    }
		    else {
		        xhr = new XMLHttpRequest();
		    }
		    return xhr;
		},
		addClass = function (node, cls) {
		    if (typeof (node) === "undefined" || node === null) {
		        return;
		    }
		    if (Object.prototype.toString.call(node) === "[object Array]") {
		        for (var i = 0, iMax = node.length; i < iMax; i++) {
		            var n = node[i];
		            n.className = n.className + " " + cls;
		        }
		        return;
		    }
		    node.className = node.className + " " + cls;
		},
		removeClass = function (node, cls) {
		    if (typeof (node) === "undefined" || node === null) {
		        return;
		    }
		    var reg = new RegExp("(\\s|^)" + cls + "(\\s|$)");
		    if (Object.prototype.toString.call(node) === "[object Array]") {
		        for (var i = 0, iMax = node.length; i < iMax; i++) {
		            n.className = n.className.replace(reg, " ");
		        }
		        return;
		    }
		    node.className = node.className.replace(reg, " ");
		},
        getParentOfType = function(currentNode, tagName, cls) {
            if (typeof (currentNode) === "undefined" || currentNode === null) {
                return null;
            }
            var p = currentNode.parentElement;
            while (typeof (p) !== "undefined" && p !== null) {
                if (p.tagName.toLowerCase === tagName) {
                    if (typeof(cls) !== "undefined" && cls !== null && cls.length > 0 && (" " + p.className + " ").replace(/[\n\t]/g, " ").indexOf(" " + cls + " ") > -1) {
                        return p;
                    }
                }
                p = p.parentElement;
            }
            return null;
        };

    configureRouting = function() {
        Routing.map("#!/:contentName(/:anchor)")
            .before(function() {
                elmntContent.innerHTML = "<img src=\"https://assets-cdn.github.com/images/spinners/octocat-spinner-32.gif\" alt=\"loading...\" class=\"center-block\" />";
                removeClass(Sizzle("li.active")[0], "active");
            })
            .to(function() {
                var specificPath = replaceAll(this.params.contentName.value, ".", "/") + ".html",
                    anchor = this.params.anchor.value,
                    loadUrl = urlDirectory + specificPath,
                    elmntLink = Sizzle("a[href='#!/" + this.params.contentName.value + "']"),
                    xhr = createXhr();

                xhr.onreadystatechange = function() {
                    if (xhr.readyState === 4 && xhr.status == 200) {
                        elmntContent.innerHTML = xhr.responseText;

                        SyntaxHighlighter.highlight();

                        if (typeof (anchor) !== "undefined") {
                            var elmntAnchor = doc.getElementById(anchor);
                            if (elmntAnchor !== null) {
                                elmntAnchor.scrollIntoView(true);
                            }
                        }
                    } else {
                        elmntContent.innerHTML = "<p>Sorry, it looks like an error occurred!</p><p>How about letting us know by creating an <a href=\"" + issueUrl + "\" target=\"_blank\">issue</a>?</p>"
                    }
                }
                xhr.open('GET', loadUrl, true);
                xhr.setRequestHeader("Content-type", "application/x-www-form-urlencode");
                xhr.send();

                elmntContent.className = elmntContent.className + " " + this.params.contentName.value;

                if (typeof (elmntLink) !== "undefined") {
                    var elmntLi = getParentOfType(elmntLink, "li");
                    addClass(elmntLi, "active");
                    addClass(getParentOfType(elmntLi, "li", "dropdown"), "active");
                }
            });
    };

    return {
        "Init": function () {
            configureRouting();
        }
    };
}(window, window.document, window.Sizzle);

$(function() {
    PageManager.Init();
    Routing.root("#!/index");
    Routing.listen();

    SyntaxHighlighter.defaults["gutter"] = false;
    SyntaxHighlighter.defaults["toolbar"] = false;
    SyntaxHighlighter.all();
});
