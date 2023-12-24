/**
 * Decrypt from base64 encoded string
 * @param {string} base64EncodedString
 * @returns {string} Returns raw string
 */
 function Decrypt(base64EncodedString) { // ... this needs to be export function, fuck this
    // bruh no fucking way ai did better than me, from lchzh :>
    let rcon = Array.from(new Array(256), (e, t) =>
        128 & t ? ((t << 1) & 255) ^ 27 : t << 1
    ),
        sBox = [
            82, 9, 106, 213, 48, 54, 165, 56, 191, 64, 163, 158, 129, 243, 215, 251,
            124, 227, 57, 130, 155, 47, 255, 135, 52, 142, 67, 68, 196, 222, 233, 203,
            84, 123, 148, 50, 166, 194, 35, 61, 238, 76, 149, 11, 66, 250, 195, 78, 8,
            46, 161, 102, 40, 217, 36, 178, 118, 91, 162, 73, 109, 139, 209, 37, 114,
            248, 246, 100, 134, 104, 152, 22, 212, 164, 92, 204, 93, 101, 182, 146,
            108, 112, 72, 80, 253, 237, 185, 218, 94, 21, 70, 87, 167, 141, 157, 132,
            144, 216, 171, 0, 140, 188, 211, 10, 247, 228, 88, 5, 184, 179, 69, 6,
            208, 44, 30, 143, 202, 63, 15, 2, 193, 175, 189, 3, 1, 19, 138, 107, 58,
            145, 17, 65, 79, 103, 220, 234, 151, 242, 207, 206, 240, 180, 230, 115,
            150, 172, 116, 34, 231, 173, 53, 133, 226, 249, 55, 232, 28, 117, 223,
            110, 71, 241, 26, 113, 29, 41, 197, 137, 111, 183, 98, 14, 170, 24, 190,
            27, 252, 86, 62, 75, 198, 210, 121, 32, 154, 219, 192, 254, 120, 205, 90,
            244, 31, 221, 168, 51, 136, 7, 199, 49, 177, 18, 16, 89, 39, 128, 236, 95,
            96, 81, 127, 169, 25, 181, 74, 13, 45, 229, 122, 159, 147, 201, 156, 239,
            160, 224, 59, 77, 174, 42, 245, 176, 200, 235, 187, 60, 131, 83, 153, 97,
            23, 43, 4, 126, 186, 119, 214, 38, 225, 105, 20, 99, 85, 33, 12, 125,
        ],
        invSBox = [
            98, 127, 241, 148, 33, 133, 224, 17, 200, 21, 232, 30, 99, 155, 154, 0, 0,
            28, 118, 107, 130, 108, 41, 189, 150, 87, 133, 137, 241, 154, 111, 214,
            219, 215, 7, 53, 250, 82, 231, 36, 50, 71, 15, 58, 81, 220, 149, 58, 209,
            154, 92, 235, 83, 246, 117, 86, 197, 161, 240, 223, 52, 59, 159, 9, 59,
            12, 6, 45, 193, 94, 225, 9, 243, 25, 238, 51, 162, 197, 123, 9, 235, 60,
            125, 234, 184, 202, 8, 188, 125, 107, 248, 99, 73, 80, 103, 106, 108, 137,
            4, 22, 173, 215, 229, 31, 94, 206, 11, 44, 252, 11, 112, 37, 91, 23, 44,
            213, 227, 221, 36, 105, 158, 182, 220, 10, 215, 230, 187, 96, 234, 99,
            212, 24, 71, 180, 49, 7, 25, 122, 58, 43, 229, 113, 74, 14, 130, 180, 250,
            126, 97, 105, 222, 23, 255, 223, 2, 29, 40, 57, 185, 125, 232, 53, 43, 44,
            175, 129, 26, 43, 182, 251, 32, 0, 83, 138, 106, 14, 111, 202, 248, 213,
            14, 163, 38, 194, 241, 124, 36, 223, 217, 69, 157, 162, 166, 107, 17, 25,
            9, 234, 11, 50, 191, 17, 43, 50, 236, 155, 65, 60, 161, 222, 123, 62, 175,
            125, 93, 252, 94, 1, 121, 35, 135, 68, 228, 129, 253, 2, 29, 14, 244, 232,
            22, 60, 75, 249, 61, 14, 167, 98, 124, 50,
        ],
        initialVector = [
            190, 86, 22, 127, 131, 218, 59, 239, 239, 248, 24, 97, 165, 197, 243, 205,
        ],
        decodedString = atob(base64EncodedString)
            .split("")
            .map((e) => e.charCodeAt());
    return new TextDecoder().decode(
        Uint8Array.from(
            (function (cipherText) {
                let cipherTextCopy = cipherText.slice(),
                    cipherTextLength = cipherTextCopy.length,
                    state = initialVector;
                for (let e = 0; e < cipherTextLength; e += 16) {
                    let block = cipherTextCopy.slice(e, e + 16);
                    addRoundKey(block, invSBox.slice(224, 240)),
                        shiftRows(block),
                        subBytes(block);
                    for (let e = 208; e >= 16; e -= 16)
                        addRoundKey(block, invSBox.slice(e, e + 16)),
                            mixColumns(block),
                            shiftRows(block),
                            subBytes(block);
                    addRoundKey(block, invSBox.slice(0, 16));
                    for (let e = 0; e < 16; e++) block[e] ^= state[e];
                    for (let t = 0; t < 16; t++) state[t] = cipherTextCopy[e + t];
                    for (let n = 0; n < 16; n++) cipherTextCopy[e + n] = block[n];
                }
                return cipherTextCopy.slice(
                    0,
                    -cipherTextCopy[cipherTextCopy.length - 1]
                );
                function subBytes(state) {
                    for (let t = 0; t < 16; t++) state[t] = sBox[state[t]];
                }
                function shiftRows(state) {
                    let tempArray = Array.from(new Array(16), (e, t) => (13 * t) & 15),
                        tempState = state.slice();
                    for (let i = 0; i < 16; i++) state[i] = tempState[tempArray[i]];
                }
                function addRoundKey(state, roundKey) {
                    for (let n = 0; n < 16; n++) state[n] ^= roundKey[n];
                }
                function mixColumns(state) {
                    for (let n = 0; n < 16; n += 4) {
                        let i0 = state[n + 0],
                            i1 = state[n + 1],
                            i2 = state[n + 2],
                            i3 = state[n + 3],
                            sBoxColumnSum = i0 ^ i1 ^ i2 ^ i3,
                            rconColumnSum = rcon[sBoxColumnSum],
                            doubleRconColumnSumI0I2 =
                                rcon[rcon[rconColumnSum ^ i0 ^ i2]] ^ sBoxColumnSum,
                            doubleRconColumnSumI1I3 =
                                rcon[rcon[rconColumnSum ^ i1 ^ i3]] ^ sBoxColumnSum;
                        (state[n + 0] ^= doubleRconColumnSumI0I2 ^ rcon[i0 ^ i1]),
                            (state[n + 1] ^= doubleRconColumnSumI1I3 ^ rcon[i1 ^ i2]),
                            (state[n + 2] ^= doubleRconColumnSumI0I2 ^ rcon[i2 ^ i3]),
                            (state[n + 3] ^= doubleRconColumnSumI1I3 ^ rcon[i3 ^ i0]);
                    }
                }
            })(decodedString)
        )
    );
}