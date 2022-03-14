// var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

//Disable send button until connection is established



function print({ message, username }) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = username + " says " + msg;
    var html =
        `
        <div class="item-chat">
            <span>${encodedMsg}</span>
        </div>
        `;
    $("#messagesList").append(html);
}


$(document).ready(() => {
    // document.getElementById("sendButton").disabled = true;
    InitHub();
    connection.on("ReceiveMessage", function(username, message) {
        // var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        // var encodedMsg = user + " says " + msg;
        // var span = document.createElement("div");
        // span.textContent = encodedMsg;
        // span.setAttribute("class", "item-chat text-break");
        // document.getElementById("messagesList").appendChild(span);

        // var html =
        //     `
        // <div class="item-chat text-break">
        //     <span>${encodedMsg}</span>
        // </div>
        // `;
        // $("#messagesList").append(html);
        if (username == undefined || username == null) {
            username = "unknown";
        }

        if (message == undefined || message == null) {
            message = "";
        }
        print({ message, username });

    });

    connection.start().then(function() {
        // document.getElementById("sendButton").disabled = false;
    }).catch(function(err) {
        return console.error(err.toString());
    });

    document.getElementById("send-message").addEventListener("click", function(event) {
        var username = document.getElementById("username-input").value;
        var message = document.getElementById("messageInput").value;
        sendMessage({ message, username });
        // connection.invoke("SendMessage", user, message).catch(function(err) {
        //     return console.error(err.toString());
        // });
        event.preventDefault();
    });
});

function sendMessage({ message, username }) {
    let optionMessage = $("#optionMessage").val();
    switch (optionMessage) {
        case "all":
            connection.invoke("SendMessage", username, message).catch(function(err) {
                return console.error(err.toString());
            });
            break;
        case "other":
            connection.invoke("SendMessageOther", username, message).catch(function(err) {
                return console.error(err.toString());
            });
            break;
        case "myself":
            connection.invoke("SendMessageCaller", username, message).catch(function(err) {
                return console.error(err.toString());
            });
            break;

        default:
            break;
    }
}


function InitHub() {
    connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    if (connection) {
        connection.on("connected", function(message) {
            print(message);
        });


        connection.on("disconnected", function(message) {
            print(message);
        });

        connection.on("userOnline", function(message) {
            $("#user-online-amount").html(message);
        });
    }
}