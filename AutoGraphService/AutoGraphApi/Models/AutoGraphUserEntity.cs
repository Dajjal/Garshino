using System.ComponentModel.DataAnnotations.Schema;

namespace AutoGraphApi.Models;

[Table("auto_graph_users")]
public record AutoGraphUserEntity(
    [property: Column("telegram_chat_id")] string TelegramChatId,
    [property: Column("user_name")] string UserName,
    [property: Column("full_name")] string FullName
) : AbstractEntity;