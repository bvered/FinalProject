function transformTypedCharacter(typedChar) {
    return traslation[typedChar];
}

function insertTextAtCursor(text) {
    var sel, range, textNode;
    if (window.getSelection) {
        sel = window.getSelection();
        if (sel.getRangeAt && sel.rangeCount) {
            range = sel.getRangeAt(0).cloneRange();
            range.deleteContents();
            textNode = document.createTextNode(text);
            range.insertNode(textNode);

            // Move caret to the end of the newly inserted text node
            range.setStart(textNode, textNode.length);
            range.setEnd(textNode, textNode.length);
            sel.removeAllRanges();
            sel.addRange(range);
        }
    } else if (document.selection && document.selection.createRange) {
        range = document.selection.createRange();
        range.pasteHTML(text);
    }
}


$("#editable").keypress(function (evt) {
    if (evt.which) {
        var charStr = String.fromCharCode(evt.which);
        var transformedChar = transformTypedCharacter(charStr.toLowerCase());
        if (transformedChar != charStr) {
            insertTextAtCursor(transformedChar);
            return false;
        }
    }
});


var traslation = [
    { "a": 'ש' },
    { "b": 'נ' },
    { "c": 'ב' },
    { "d": 'ג' },
    { "e": 'ק' },
    { "f": 'כ' },
    { "g": 'ע' },
    { "h": 'י' },
    { "i": 'ן' },
    { "j": 'ח' },
    { "k": 'ל' },
    { "l": 'ך' },
    { "m": 'צ' },
    { "n": 'מ' },
    { "o": 'ם' },
    { "p": 'פ' },
    { "q": '/' },
    { "r": 'ר' },
    { "s": 'ד' },
    { "t": 'א' },
    { "u": 'ו' },
    { "v": 'ה' },
    { "w": '' },
    { "x": 'ס' },
    { "y": 'ט' },
    { "z": 'ז' },
    { ",": 'ת' },
    { ".": 'ץ' },
    { ";": 'ף' },
];
