angular.module('ngBoilerplate')

    .factory('authService', ['$http', '$q', 'localStorageService', 'CONSTANTS', function ($http, $q, localStorageService, CONSTANTS) {

        var serviceBase = CONSTANTS.BASE_API_URL;
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: ''
        };

        var _signup = function (signupData) {

            _logOut();

            return $http
                .post(serviceBase + 'api/accounts/signup', signupData)
                .then(function (response) {
                    return response;
                });
        };

        var _login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.username + "&password=" + loginData.password;

            var deferred = $q.defer();

            $http
                .post(serviceBase + 'oauth/token', data, {headers: {'Content-Type': 'application/x-www-form-urlencoded'}})
                .success(function (response) {

                    var expiresIn = response.expires_in;
                    var tokenType = response.token_type;
                    var accessToken = response.access_token;

                    localStorageService.set('authorizationData', {
                        token: accessToken,
                        username: loginData.username
                    });

                    _authentication.isAuth = true;
                    _authentication.username = loginData.username;

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });

            return deferred.promise;
        };

        var _logOut = function () {

            localStorageService.remove('authorizationData');

            _authentication.isAuth = false;
            _authentication.username = '';
        };

        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.username = authData.username;
            }
        };

        authServiceFactory.signup = _signup;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;

        return authServiceFactory;
    }]);