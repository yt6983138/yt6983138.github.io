/**
 * Eval code in string
 * @param {string} strToEval
 * @param {boolean} useFunctionInstead
 * @param {boolean} useStrict
 * @returns {any}
 */
function EvalCode(strToEval, useFunctionInstead, useStrict) {
    if (useStrict) {
        "use strict";
    }
    if (useFunctionInstead) {
        return Function("return " + strToEval)();
    } else {
        return eval(strToEval);
    }
}