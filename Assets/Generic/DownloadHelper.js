/**
 * Download file from url supplied
 * @param {string} url
 * @param {string} filename
 * @returns {void}
 */
function downloadFromUrl(url, filename) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = filename ?? '';
    anchorElement.click();
    anchorElement.remove();
}
/**
 * Download in-memory file, encoded by base64
 * @param {string} base64data
 * @param {string} filename
 * @param {string} contentType
 * @returns {void}
 */
function downloadFromData(base64data, filename, contentType) {
    const numArray = atob(base64data).split('').map(c => c.charCodeAt(0));
    const uint8Array = new Uint8Array(numArray);
    const blob = new Blob([uint8Array], { type: contentType });
    const url = URL.createObjectURL(blob);
    downloadFromUrl(url, filename);
}