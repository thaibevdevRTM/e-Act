function ConsentManager (osanoOptions, consentManagerOptions) {
    var o = consentManagerOptions;
    if (!o) o = {};

    // Example of cookieList
    // ['_ga', /^_pk.*/] <-- name only
    // [['_ga', '/'], [/^_pk.*/, '/']] <-- array of name, path
    this._cookieList = typeof o.cookieList !== 'undefined' ? o.cookieList : [/.*/]; 
    this._scriptExecuted = false;

    osanoOptions.type = 'opt-in';
    osanoOptions.revokeBtn = "<div id=\"cookieconsent-revoke-button\" class=\"cc-revoke\" style=\"padding:0\"></div>";
    
    if (!osanoOptions.cookie)
        osanoOptions.cookie = {};

    if (!osanoOptions.cookie.name)
        osanoOptions.cookie.name = 'cookieconsent_status';

    this._cookieName = osanoOptions.cookie.name;

    var me = this;

    osanoOptions.onInitialise = function (status) {
        var didConsent = this.hasConsented();
        
        if (didConsent) {
            me._updateElements(true);
        }
        else {
            me._deleteCookies();
        }
    };

    osanoOptions.onStatusChange = function(status, chosenBefore) {
        var didConsent = this.hasConsented();
        me._updateElements(didConsent);

        if (!didConsent) {
            me._deleteCookies();
        }
    };

    osanoOptions.onRevokeChoice = function() {
        me._updateElements(false);
        me._deleteCookies();
    };

    window.cookieconsent.initialise(osanoOptions);
}

ConsentManager.prototype._updateElements = function (consent) {
    var elementList, i, len, element;

    elementList = document.querySelectorAll('[data-src]');

    for (i = 0, len = elementList.length; i < len; i++) {
        element = elementList[i];
        var dataSrc = element.getAttribute('data-src');
        if (consent)
            element.setAttribute('src', dataSrc);
        else
            element.removeAttribute('src');
    }

    if (consent && !this._scriptExecuted) {
        elementList = document.querySelectorAll('script[data-type]');

        for (i = 0, len = elementList.length; i < len; i++) {
            element = elementList[i];
            var dataType = element.getAttribute('data-type');
            var newScript = document.createElement('script');

            for (var j = 0, lenJ = element.attributes.length; j < lenJ; j++) {
                var attr = element.attributes[j];
                newScript.setAttribute(attr.name, attr.value);
            }

            if (dataType === 'default')
                newScript.removeAttribute('type');
            else
                newScript.setAttribute('type', dataType);
                
            newScript.innerHTML = element.innerHTML;

            var parent = element.parentElement;
            parent.insertBefore(newScript, element);
            parent.removeChild(element);
        }
        this._scriptExecuted = true;
    }
}

ConsentManager.prototype._deleteCookies = function () {
    for (var i = 0, len = this._cookieList.length; i < len; i++) {
        var c = this._cookieList[i];
        if (!Array.isArray(c))
            c = [c];

        var cookieNames = this._getMatchedCookieNames(c[0]);
        for (var j = 0, len1 = cookieNames.length; j < len1; j++) {
            this._deleteCookie(cookieNames[j], c[1])
        }
    }
}

ConsentManager.prototype._getAllCookieNames = function () {
    var cookieNames = [];
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookies = decodedCookie.split(';');
    for (var i = 0, len = cookies.length; i < len; i++) {
        var cookie = cookies[i];
        var a = cookie.split('=');
        var cookieName = a[0].trim();

        if (cookieName !== this._cookieName)
            cookieNames.push(cookieName);
    }
    return cookieNames;
}

ConsentManager.prototype._getMatchedCookieNames = function (s) {
    var isRegExp = s instanceof RegExp; 
    var cookieNames = [];
    var allCookieNames = this._getAllCookieNames();
    for (var i = 0, len = allCookieNames.length; i < len; i++) {
        var name = allCookieNames[i];

        if (!isRegExp) {
            if (name === s) {
                cookieNames.push(name);
                break;
            }
        }
        else {
            if (s.test(name)) {
                cookieNames.push(name);
            }
        }
    }
    return cookieNames;
}

ConsentManager.prototype._deleteCookie = function (name, path) {
    var c = [];
    c.push(name + '=');
    c.push('expires=Thu, 01 Jan 1970 00:00:00 UTC');

    if (!path)
        path = '/';

    c.push('path=' + path);

    document.cookie = c.join('; ');
}

// public function
ConsentManager.prototype.revokeConsent = function () {
    document.getElementById('cookieconsent-revoke-button').click();
}