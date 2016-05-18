(function () {

    var app = angular.module('chat-app', []);

    app.controller('rooms', function ($scope, $http) {
        $scope.newItem = '';
        $scope.newMessage = '';
        $scope.message = [];
        $scope.rooms = [];

        $http.get('/api/Chat/Get').success(function (data) {
            $scope.rooms = data;
        }) //HTTP GET for the rooms API


        $scope.chatHub = $.connection.chatHub //instantiates hub

        $.connection.hub.start(); //connects and starts hub

        $scope.appendRooms = function (oldRoomName) {
            var newElem = $scope.newItem;
            var hasElem = false;
            $.each($scope.rooms, function (i, obj) {
                if (obj.RoomName == newElem) { hasElem = true; return false }
            });
            if (hasElem == true) { //Appears to be working to test if the room already exists.
                alert('This room already exists.')
                $scope.welcome = 'Please select or create a room';
                $scope.newItem = '';
            }
            else if (hasElem == false) {
                $scope.chatHub.server.joinRoom(newElem, oldRoomName);
            }
        }; //Adds new rooms

        $scope.showNewMessage = function () {
            var newMessage = $scope.newMessage;
            $scope.chatHub.server.writeMessage({ roomName: $scope.roomDropDown.RoomName, id: $scope.roomDropDown.Id }, { UserName: $scope.username.text, Message: newMessage });
            $scope.newMessage = '';
        }; //sends message to signalr

        $scope.selectRoom = function (oldRoomName) {
            $scope.welcome = 'Welcome to: ';
            $scope.chatHub.server.joinRoom($scope.roomDropDown.RoomName, oldRoomName);
        }; //switches users into rooms

        $scope.chatHub.client.addMessage = function (dateCreated, userName, msg, groupName) {
            $scope.message.push({ "DateCreated": dateCreated, "UserName": userName, "Message": msg });
            $scope.$apply();
        }; //adds message to clients

        $scope.chatHub.client.sendNewRoom = function (id, roomname) {
            $scope.rooms.splice(0, 0, { "Id": id, "RoomName": roomname });
            $scope.roomDropDown = $scope.rooms[0];
            $scope.welcome = 'Welcome to: ';
            $scope.newItem = '';
            $scope.$apply();
        }; //update for single client who created new room

        $scope.chatHub.client.addNewRoom = function (id, roomname) {  //update for all other users
            $scope.rooms.splice(0, 0, { "Id": id, "RoomName": roomname });
            $scope.$apply();
        }; //updates rooms for the rest of the clients

        $scope.chatHub.client.sendRoomMessages = function (commentsNeeded) {
            $scope.message = commentsNeeded;
            $scope.$apply();
        }; //sends new messages to clients in each group

    });
}());
