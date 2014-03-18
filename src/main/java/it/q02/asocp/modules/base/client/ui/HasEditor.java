package it.q02.asocp.modules.base.client.ui;

import com.google.gwt.editor.client.Editor;

/**
 * User: aleksander at  18.03.14, 18:06
 */
public interface HasEditor<T> {

    public Editor createEditor(T editor);

}
