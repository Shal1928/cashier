package it.q02.asocp.modules.base.client.ui;

import com.google.gwt.user.client.ui.Image;
import com.google.gwt.user.client.ui.IsWidget;

import java.util.List;

/**
 * User: aleksander at  18.03.14, 18:00
 * Комманда способная изменить что-то в DataPanel
 */
public interface ChangeCommand<T> {

    public final class CommandContext{

        private List affectedObjects;
        private DataPanel panel;

        public CommandContext(List affectedObjects, DataPanel panel) {
            this.affectedObjects = affectedObjects;
            this.panel = panel;
        }
    }

    public static abstract class  CommandCaption implements IsWidget {

        private CommandContext commandContext;

        private final ChangeCommand command;

        private boolean systemEnabled;

        protected CommandCaption(ChangeCommand command) {
            this.command = command;
        }

        protected abstract void contextChanged(CommandContext context);

        public abstract void setEnabled(boolean isEnabled);

        public void setCommandContext(CommandContext newContext){
            this.commandContext = newContext;
            contextChanged(newContext);
        }

        public final void executeCommand(){
            command.execute(commandContext.panel,commandContext.affectedObjects);
        }
    }


    /**
     * Виджет описывающий комманду
      * @return
     */
    public CommandCaption getWidget();

    /**
     * Взывается
     * @param panel
     * @param data
     */
    public void execute(DataPanel<T> panel,List<T> data);

}
