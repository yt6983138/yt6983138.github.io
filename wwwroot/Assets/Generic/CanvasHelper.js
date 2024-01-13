var canvasHolder = {};
var ctxHolder = {};
var imageHolder = {};
var hiddenImageDiv = document.getElementById("HiddenImageHolder");

function GetCanvasByElementID(id, storeName) {
    canvasHolder[storeName] = document.getElementById(id);
    ctxHolder[storeName] = canvasHolder[storeName].getContext("2d");
}

function CanvaDrawImg(ctxID, imageName, sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight) {
    ctxHolder[ctxID].drawImage(imageHolder[imageName], sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight);
}
function AddImageByUrl(url, id) {
    let img = document.createElement("img");
    img.src = url;
    hiddenImageDiv.appendChild(img);
    imageHolder[id] = img;
}
function RemoveImage(id) {
    imageHolder[id] = undefined;
}

/**
 * set property
 * @param {string} id
 * @param {string} propertyName
 * @param {any} value
 */
function AssignCtxProperty(id, propertyName, value) { // https://stackoverflow.com/a/13719799
    if (typeof propertyName === "string")
        propertyName = propertyName.split(".");

    if (propertyName.length > 1) {
        var e = propertyName.shift();
        AssignCtxProperty(ctxHolder[id][e] =
            Object.prototype.toString.call(ctxHolder[id][e]) === "[object Object]"
                ? ctxHolder[id][e]
                : {},
            propertyName,
            value);
    } else
        ctxHolder[id][propertyName[0]] = value;
}
function RunCtxFunc(id, funcName, ...Args) {
    return ctxHolder[id][funcName](...Args);
}