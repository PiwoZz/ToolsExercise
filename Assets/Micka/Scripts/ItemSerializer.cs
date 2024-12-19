using Newtonsoft.Json;

public static class ItemSerializer {
    public static string SerializeItem(Item item) {
        return JsonConvert.SerializeObject(item);
    }

    public static Item DeserializeItem(Item item) {
        return JsonConvert.DeserializeObject<Item>(item.ToString());
    }
}
