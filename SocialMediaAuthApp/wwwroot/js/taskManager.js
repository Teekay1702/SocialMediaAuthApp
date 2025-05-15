const connection = new signalR.HubConnectionBuilder()
    .withUrl("/taskHub")
    .build();

// Ensure loadTasks is called on SignalR update
connection.on("ReceiveTaskUpdate", () => loadTasks());
connection.on("TaskDeleted", () => loadTasks());


connection.start().catch(err => console.error(err.toString()));

document.getElementById("addTaskButton").addEventListener("click", () => {
    const id = document.getElementById("taskId").value;
    const title = document.getElementById("taskTitle").value;
    const description = document.getElementById("taskDescription").value;

    const task = { id: parseInt(id || "0"), title, description, isCompleted: false };

    const url = task.id ? "/Home/EditTask" : "/Home/AddTask";

    fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(task)
    }).then(() => {
        window.location.reload();
    });
});

function clearForm() {
    document.getElementById("taskId").value = "";
    document.getElementById("taskTitle").value = "";
    document.getElementById("taskDescription").value = "";
}

function loadTasks() {
    fetch("/Home/Index")
        .then(response => response.text())
        .then(html => {
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, "text/html");
            const newTaskList = doc.getElementById("taskList").innerHTML;
            document.getElementById("taskList").innerHTML = newTaskList;
            bindButtons(); // Rebind events
        });
}

function bindButtons() {
    document.querySelectorAll(".delete-task").forEach(btn => {
        btn.addEventListener("click", () => {
            const id = btn.closest(".task-item").dataset.id;
            fetch("/Home/DeleteTask", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(parseInt(id))
            })
                .then(() => loadTasks());

        });
    });

    document.querySelectorAll(".edit-task").forEach(btn => {
        btn.addEventListener("click", () => {
            const item = btn.closest(".task-item");
            const id = item.dataset.id;
            const title = item.querySelector(".task-title").innerText;
            const desc = item.querySelector(".task-desc").innerText;

            document.getElementById("taskId").value = id;
            document.getElementById("taskTitle").value = title;
            document.getElementById("taskDescription").value = desc;

            const modal = new bootstrap.Modal(document.getElementById("addTaskModal"));
            modal.show();
        });

    });
}

document.addEventListener("DOMContentLoaded", bindButtons);
