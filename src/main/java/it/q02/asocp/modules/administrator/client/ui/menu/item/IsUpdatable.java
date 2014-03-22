package it.q02.asocp.modules.administrator.client.ui.menu.item;

/**
 * User: aleksander at  22.03.14, 22:31
 * элементы меню имеющие не стандратный интерфейс(Реализуюшие интерфейс HasCustomRenderer), могут
 * использовать этот класс если из необходимо время от вермени перерисовывать
 */
public interface IsUpdatable {

    public void updateItem();

}
