# ActivDbgNET
An implementation of the ActivDbg COM interface for C#

This implementation will be used to interface between ActiveScript Debugging COM interface and a Typescript extension https://github.com/Luncher91/VBScript-vscode to be able to debug VBScript code in VSCode.

## ToDo
* exploring objects
* executing inline code
* add debugger to MDM - list program as JIT
* define an interface for vscode plugin

## Done
* attach to process
* stop on breakpoint, resume, step over, step in, step out
* list loaded documents
* show content of documents
* scanner attributes for documents
* next execution step position