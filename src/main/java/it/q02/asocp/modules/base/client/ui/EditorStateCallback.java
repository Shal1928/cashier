package it.q02.asocp.modules.base.client.ui;

/**
 * User: aleksander at  24.03.14, 20:31
 */
public interface EditorStateCallback<T> {

    public static interface MessageNotifer{

        public void addMessage(String message);
        public void showMessages();
        public void clear();
    }

    public void onBeginEdit(T editedObject);

    public void onCommitEdit(T editedObject, MessageNotifer notifiter);

    public void onCancelEdit(T editedObject);




}
