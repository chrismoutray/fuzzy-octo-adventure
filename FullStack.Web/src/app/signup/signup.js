angular.module('ngBoilerplate.signup', [
    'ui.router',
    'placeholders',
    'ui.bootstrap'
])
    .config(function config($stateProvider) {

        $stateProvider
            .state('signup', {
                url: '/signup',
                views: {
                    "main": {
                        controller: 'SignUpCtrl',
                        templateUrl: 'signup/signup.tpl.html'
                    }
                },
                data: {pageTitle: 'Sign Up'}
            })
            .state('confirm-signup', {
                url: '/confirm-signup?user&token',
                views: {
                    "main": {
                        controller: 'ConfirmSignUpCtrl',
                        templateUrl: 'signup/confirm-signup.tpl.html'
                    }
                },
                data: {pageTitle: 'Confirm Sign Up'},
                resolve: {
                    isConfirmSuccessful: ['$stateParams', '$location', 'authService', function ($stateParams, $location, authService) {

                        if ($stateParams.user === undefined || $stateParams.token === undefined)
                            return false;

                        var vm = {
                            username: $stateParams.user,
                            password: $stateParams.token
                        };

                        authService
                            .login(vm)
                            .then(function (response) {
                                $location.path('/confirm-details');

                                // todo remove username and token from uri

                                // now we've logged in started after-signup wizard
                                // in order to confirm and complete user details and guide them through any initial app settings

                            },
                            function (err) {
                                $scope.message = err.error_description;
                            });
                    }]
                }
            })
        ;
    })

    .controller('SignUpCtrl', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

        $scope.vm = {
            firstName: '',
            lastName: '',
            displayName: '',
            email: '',
            emailConfirmation: '',
            password: '',
            passwordConfirmation: '',
            isTermsAgreed: false
        };

        $scope.message = "";

        $scope.signup = function () {

            console.log('signup');

            authService
                .signup($scope.vm)
                .then(function (response) {
                    $location.path('/home');
                },
                function (err) {
                    $scope.message = err.error_description;
                });
        };

    }])

    .controller('ConfirmSignUpCtrl', ['$scope', function ($scope) {

    }])

    //.controller('LogoutCtrl', function ($scope, $location, authService) {
    //    authService.logOut();
    //    $location.path('/home');
    //})

;
