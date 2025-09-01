const apiUrl = "/api/tasks";

async function loadTasks() {
    const res = await fetch(apiUrl);
    const tasks = await res.json();

    const list = document.getElementById("taskList");
    list.innerHTML = "";

    tasks.forEach(task => {
        const div = document.createElement("div");
        div.className = "task " + (task.isCompleted ? "completed" : "");
        div.innerHTML = `
            <b>${task.title}</b> - ${task.description} 
            (Assigned to: ${task.assignedTo})
            <br>
            <button onclick="markCompleted(${task.id})">Complete</button>
            <button onclick="deleteTask(${task.id})">Delete</button>
        `;
        list.appendChild(div);
    });
}

async function addTask() {
    const title = document.getElementById("title").value;
    const description = document.getElementById("description").value;
    const assignedTo = document.getElementById("assignedTo").value;

    await fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, description, assignedTo, isCompleted: false })
    });

    loadTasks();
}

async function markCompleted(id) {
    const res = await fetch(`${apiUrl}/${id}`);
    const task = await res.json();
    task.isCompleted = true;

    await fetch(`${apiUrl}/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(task)
    });

    loadTasks();
}

async function deleteTask(id) {
    await fetch(`${apiUrl}/${id}`, { method: "DELETE" });
    loadTasks();
}

// Load tasks on page start
loadTasks();
