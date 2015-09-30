angular.module('ngBoilerplate', [
    'LocalStorageModule',
    'ui.router',
    'templates-app',
    'templates-common',
    'ngBoilerplate.home',
    'ngBoilerplate.about',
    'ngBoilerplate.signup',
    'ngBoilerplate.login',
    'ngBoilerplate.users'
])

    .constant('CONSTANTS', {
        BASE_API_URL: 'http://api.fullstack.co.uk/'
        //BASE_API_URL: 'http://localhost:49566/'
    })

    .config(function myAppConfig($stateProvider, $urlRouterProvider, $httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
        $urlRouterProvider.otherwise('/home');
    })

    .run(['authService', function run(authService) {
    }])

    .controller('AppCtrl', function AppCtrl($scope, $location, authService) {

        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            if (angular.isDefined(toState.data.pageTitle)) {
                $scope.pageTitle = toState.data.pageTitle + ' | ngBoilerplate';
            }
        });

        $scope.logout = function () {
            authService.logOut();
            $location.path('/home');
        };
    })

;
