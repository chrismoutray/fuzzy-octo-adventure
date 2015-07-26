angular.module('ngBoilerplate.login', [
    'ui.router',
    'placeholders',
    'ui.bootstrap'
])
    .config(function config($stateProvider) {

        $stateProvider
            .state('login', {
                url: '/login',
                views: {
                    "main": {
                        controller: 'LoginCtrl',
                        templateUrl: 'login/login.tpl.html'
                    }
                },
                data: {pageTitle: 'Login'}
            })

            //.state('logout', {
            //    controller: ['$scope', '$location', 'authService', function ($scope, $location, authService) {
            //        authService.logOut();
            //        $location.path('/home');
            //    }]
            //})

            //.state('logout', {
            //    url: '/logout',
            //    views: {
            //        "main": {
            //            controller: 'LogoutCtrl',
            //            templateUrl: 'login/logout.tpl.html'
            //        }
            //    },
            //    data: {pageTitle: 'Logout'}
            //})
        ;

        console.log('### ngBoilerplate.login configured');
    })

    .controller('LoginCtrl', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

        $scope.vm = {
            username: '',
            password: ''
        };

        $scope.message = "";

        $scope.login = function () {

            authService
                .login($scope.vm)
                .then(function (response) {
                    $location.path('/home');
                },
                function (err) {
                    $scope.message = err.error_description;
                });
        };

    }])

    //.controller('LogoutCtrl', function ($scope, $location, authService) {
    //    authService.logOut();
    //    $location.path('/home');
    //})

;