package it.q02.asocp.modules.base.client.ui;

/**
 * User: aleksander at  24.03.14, 21:25
 */
public class EditorStateCallbackProxy<T> implements EditorStateCallback<T>{

    private EditorStateCallback<T> logicCallBack;

    private final EditorStateCallback<T> informationCallBack;

    public EditorStateCallbackProxy(EditorStateCallback<T> informationCallBack) {
        this.informationCallBack = informationCallBack;
    }

    public void setLogicCallBack(EditorStateCallback<T> logicCallBack) {
        this.logicCallBack = logicCallBack;
    }

    @Override
    public void onBeginEdit(T editedObject) {
        informationCallBack.onBeginEdit(editedObject);
        logicCallBack.onBeginEdit(editedObject);
    }

    @Override
    public void onCommitEdit(T editedObject, MessageNotifer notifiter) {
        informationCallBack.onCommitEdit(editedObject,notifiter);
        logicCallBack.onCommitEdit(editedObject,notifiter);
    }

    @Override
    public void onCancelEdit(T editedObject) {
        informationCallBack.onBeginEdit(editedObject);
        logicCallBack.onBeginEdit(editedObject);
    }

    public EditorStateCallback callWith(EditorStateCallback callback){
        setLogicCallBack(callback);
        return this;
    }
}
