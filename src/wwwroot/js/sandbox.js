void (function initializeSandbox() {
    const defaultCode = `procedure Main() => void
{
    var point = Point(2.6, 3.4, 4.3);
    var v = point.Sum();
    print(string(v));
}

class Point
{
    var x = 0.0;
    var y = 0.0;
    var z = 0.0;
}

procedure Point.Sum() => float32
{
    return this.x + this.y + this.z;
}`;

    const sandbox = document.getElementById("sandbox");

    const codeInput = document.getElementById("sandbox-code-input");

    const output = document.getElementById("sandbox-output");
    const outputResults = output.querySelector(".results");
    const outputErrors = output.querySelector(".errors");
    const outputWaiting = output.querySelector(".waiting");

    const runButton = document.getElementById("sandbox-run-btn");

    function setOutputWaiting(waiting) {
        outputWaiting.classList.toggle("active", waiting);
        outputResults.classList.toggle("active", !waiting);
    }
    function setOutputResult(result) {
        setOutputWaiting(false);

        // TODO: Determine errors.
        const isError = false;
        outputResults.classList.toggle("hasErrors", isError);
        outputResults.querySelector("pre>code").innerText = result.output;
    }

    function setRunButtonActive(active) {
        runButton.toggleAttribute("disabled", !active);
    }

    if (!codeInput.value) {
        if (window.location.hash) {
            // Decode the encoded program (base 64) from the URL hash.
            const encoded = window.location.hash.slice(1);
            const decoded = atob(encoded);
            codeInput.value = decoded;
        } else {
            codeInput.value = defaultCode;
        }
    }

    // Create the text editor.
    const codeMirror = CodeMirror.fromTextArea(codeInput, {
        theme: "darcula",
        mode: "javascript",
        lineNumbers: true,
        indentUnit: 4,
        indentWithTabs: true,
    });
    codeMirror.on("change", function (cm) {
        codeInput.value = cm.getValue();
    });

    CodeMirror.commands.save = function (cm) {
        window.location.hash = btoa(cm.getValue());
    };

    runButton.addEventListener("click", function (e) {
        e.preventDefault();

        const body = {
            main: codeInput.value,
        };

        setRunButtonActive(false);
        setOutputWaiting(true);

        fetch("/sandbox/run", {
            method: "post",
            body: JSON.stringify(body),
            headers: {
                "content-type": "application/json",
            },
        })
            .then((response) => {
                return response.json();
            })
            .then((result) => {
                setOutputResult(result);
                setRunButtonActive(true);
            })
            .catch((error) => {
                console.error(error);
                setRunButtonActive(true);
            });
    });
})();
