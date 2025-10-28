using Godot;

namespace SharpIDE.Godot.Features.CodeEditor;

public partial class SharpIdeCodeEdit
{
    // public override void _Backspace(int caretIndex)
    // {
    // 	//base._Backspace(caretIndex);
    // 	var caretLine = GetCaretLine(caretIndex);
    // 	var caretCol = GetCaretColumn(caretIndex);
    // 	RemoveText(caretLine, caretCol - 1, caretLine, caretCol);
    // }
    
    public override void _HandleUnicodeInput(int unicodeChar, int caretIndex)
    {
        StartAction(EditAction.Typing);
        string charStr = char.ConvertFromUtf32(unicodeChar);
        InsertTextAtCaret(charStr, caretIndex);
        var codeCompletionSelectedIndex = GetCodeCompletionSelectedIndex();
        var isCodeCompletionPopupOpen = codeCompletionSelectedIndex is not -1;
        if (isCodeCompletionPopupOpen && charStr == " ")
        {
            //CancelCodeCompletion();
            Callable.From(() => CancelCodeCompletion()).CallDeferred();
        }
        else if (isCodeCompletionPopupOpen is false && _codeCompletionTriggers.Contains(charStr, StringComparer.OrdinalIgnoreCase))
        {
            // This is hopes and prayers that OnTextChanged has finished updating the document in the workspace...
            //Callable.From(() => EmitSignalCodeCompletionRequested()).CallDeferred();
            Callable.From(() => RequestCodeCompletion(true)).CallDeferred();
        }
        EndAction();
    }
    
