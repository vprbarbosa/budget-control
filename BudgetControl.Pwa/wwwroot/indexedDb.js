const dbName = "BudgetControlDb";
const dbVersion = 1;
const storeName = "snapshots";

function openDb() {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName, dbVersion);

        request.onupgradeneeded = e => {
            const db = e.target.result;
            if (!db.objectStoreNames.contains(storeName)) {
                db.createObjectStore(storeName);
            }
        };

        request.onsuccess = e => resolve(e.target.result);
        request.onerror = e => reject(e.target.error);
    });
}

async function saveSnapshot(key, json) {
    const db = await openDb();
    const tx = db.transaction(storeName, "readwrite");
    tx.objectStore(storeName).put(json, key);
}

async function loadSnapshot(key) {
    const db = await openDb();
    const tx = db.transaction(storeName, "readonly");
    const request = tx.objectStore(storeName).get(key);

    return new Promise(resolve => {
        request.onsuccess = () => resolve(request.result ?? null);
        request.onerror = () => resolve(null);
    });
}
