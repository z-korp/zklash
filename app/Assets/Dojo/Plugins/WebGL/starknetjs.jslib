mergeInto(LibraryManager.library, {
  WaitForTransaction: function (txHashPtr, optionsPtr, callbackPtr) {
    const txHash = UTF8ToString(txHashPtr);
    const options = UTF8ToString(optionsPtr);

    // Ensure starknetJs is loaded and available globally
    if (typeof starknetJs === "undefined") {
      console.error("Starknet library not loaded");
      return;
    }

    const optionsObj = JSON.parse(options);

    const provider = new starknetJs.RpcProvider({
      nodeUrl: optionsObj.nodeUrl,
    });

    provider
      .waitForTransaction(txHash, optionsObj)
      .then((receipt) => {
        console.log("Transaction Receipt: ", receipt);

        // Extract the status of the transaction
        const finalityStatus = receipt.finality_status;
        const executionStatus = receipt.execution_status;

        // Prepare the result object
        const result = {
          txHash: txHash,
          executionStatus: executionStatus,
          finalityStatus: finalityStatus,
          receipt: receipt,
        };

        const resultJson = JSON.stringify(result);
        const resultPtr = allocate(
          intArrayFromString(resultJson),
          "i8",
          ALLOC_NORMAL
        );
        dynCall("vii", callbackPtr, [1, resultPtr]); // Call success callback
      })
      .catch((error) => {
        const errorPtr = allocate(
          intArrayFromString(error.message),
          "i8",
          ALLOC_NORMAL
        );
        dynCall("vii", callbackPtr, [0, errorPtr]); // Call error callback
      });
  },

  DecodeShortString: function (str) {
    var jsString = UTF8ToString(str);
    var result;
    try {
      // Access decodeShortString from the shortString_exports object
      result = src_exports.shortString.decodeShortString(jsString);
    } catch (error) {
      console.error("Error in DecodeShortString:", error);
      result = "ERROR: " + error.message;
    }
    var bufferSize = lengthBytesUTF8(result) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(result, buffer, bufferSize);
    return buffer;
  },
});
