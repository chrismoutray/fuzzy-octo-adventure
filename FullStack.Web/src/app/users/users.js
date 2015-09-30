angular.module('ngBoilerplate.users', [
    'ui.router',
    'placeholders',
    'ui.bootstrap'
])
    .config(function config($stateProvider) {
        $stateProvider.state('users', {
            url: '/users',
            views: {
                "main": {
                    controller: 'UsersCtrl',
                    templateUrl: 'users/users.tpl.html'
                }
            },
            data: {pageTitle: 'Users'},
            resolve: {
                users: ['$http', 'CONSTANTS', function ($http, CONSTANTS) {
                    return $http.get(CONSTANTS.BASE_API_URL + 'api/accounts/users').then(function (results) {
                        return results.data;
                    });
                }]
            }
        });
    })

    .controller('UsersCtrl', ['$scope', '$http', 'CONSTANTS', 'users', function UsersCtrl($scope, $http, CONSTANTS, users) {

        var _deleteUser = function (user) {

            var req = {
                method: 'DELETE',
                url: CONSTANTS.BASE_API_URL + 'api/accounts/user/' + user.id
            };

            //
            //$http(req)
            $http['delete'](CONSTANTS.BASE_API_URL + 'api/accounts/user/' + user.id)
                .then(function (results) {
                    return results.data;
                })
                .catch(function (ex) {
                    console.log(ex);
                });
        };

        var _getUsers = function () {

            $http.get(CONSTANTS.BASE_API_URL + 'api/accounts/users')
                .then(function (results) {
                    return results.data;
                })
                .then (function (users) {
                    $scope.vm.users = users;
                });
        };

        $scope.vm = $scope.vm || {};
        $scope.vm.users = users;

        $scope.deleteUser = _deleteUser;
    }])
;