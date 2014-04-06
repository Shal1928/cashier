namespace ASofCP.Cashier.Helpers
{
    public static class ObjectExt
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool NotNull(this object obj)
        {
            return obj != null;
        }

        //TODO: Make with Expression safe call Props or Meths with null check
        //public static bool SafeCheck(this object obj, )
    }
}
