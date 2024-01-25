var canvasHolder = {};
var ctxHolder = {};
var imageHolder = {};
var imageLoadDone = {};
var hiddenImageDiv = document.getElementById("HiddenImageHolder");

function GetCanvasByElementID(id, storeName) {
    canvasHolder[storeName] = document.getElementById(id);
    console.log(document.getElementById(id));
    console.log(canvasHolder);
    console.log(canvasHolder[storeName].getContext("2d"));
    ctxHolder[storeName] = canvasHolder[storeName].getContext("2d");
    console.log(ctxHolder);
}

function CanvasDrawImg(ctxID, imageName, sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight) {
    ctxHolder[ctxID].drawImage(imageHolder[imageName], sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight);
}
function CanvasDrawImg1(ctxID, imageName, dx, dy) {
    console.log(ctxHolder);
    ctxHolder[ctxID].drawImage(imageHolder[imageName], dx, dy);
}
function CanvasDrawImg2(ctxID, imageName, dx, dy, dWidth, dHeight) {
    ctxHolder[ctxID].drawImage(imageHolder[imageName], dx, dy, dWidth, dHeight);
}
function AddImageByUrl(url, id) {
    hiddenImageDiv = document.getElementById("HiddenImageHolder");
    let img = document.createElement("img");
    img.src = url;
    let func = Function("e", "DotNet.invokeMethod('yt6983138.github.io', 'JSCallBack', '" + id + "', e.target.width, e.target.height);");
    img.onload = func;
    hiddenImageDiv.appendChild(img);
    imageHolder[id] = img;
}
function RemoveImage(id) {
    imageHolder[id] = undefined;
}
function GetCanvasHeight(id) {
    return canvasHolder[id].height;
}
function GetCanvasWidth(id) {
    return canvasHolder[id].width;
}
function GetImageWidth(id) {
    console.log(imageHolder[id].width);
    return imageHolder[id].width;
}
function GetImageHeight(id) {
    console.log(imageHolder[id].height);
    return imageHolder[id].height;
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
    console.log(Args);
    return ctxHolder[id][funcName](...Args);
}