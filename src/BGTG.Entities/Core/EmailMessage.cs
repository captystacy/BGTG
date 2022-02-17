namespace BGTG.Entities.Core
{
    /// <summary>
    /// Mail Message to send
    /// </summary>
    public class EmailMessage : IEmailMessage
    {
        public EmailMessage()
        {
            Result = new SendEmailResult();
        }

        /// <inheritdoc />
        public string MailTo { get; set; } = null!;

        /// <inheritdoc />
        public string Subject { get; set; } = null!;

        /// <inheritdoc />
        public string Body { get; set; } = null!;

        /// <summary>
        /// Sent result info
        /// </summary>
        public SendEmailResult Result { get; }

        /// <summary>
        /// Use HTML in the Body
        /// </summary>
        public bool IsHtml { get; set; }

        /// <inheritdoc />
        public string MailFrom { get; set; } = null!;
    }
}
