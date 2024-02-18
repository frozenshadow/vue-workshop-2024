/**
 * Code inspired by https://medium.com/apprendre-le-web-avec-lior/a-brief-history-of-webcomponents-and-a-tutorial-to-make-yours-a52d329913e7
 */
class EmployeeListItemElement extends HTMLElement {

    constructor() {
        super();

        const position = this.getAttribute('job-position');
        const fullName = this.getAttribute('full-name');
        const avatar = {
            service: this.getAttribute('avatar-service'),
            user: this.getAttribute('avatar-user')
        };
        const avatarUrl = `https://unavatar.io/${avatar.service}/${avatar.user}`;

        // Declare a Shadow DOM within our tag. Now the rendering will be done IN the Shadow DOM.
        const shadowRoot  = this.attachShadow({ mode: 'open' });

        // Find and copy the desired template node to later append it to the Shadow DOM.
        const template = document.querySelector('#employee-list-item-template');
        const content = document.importNode(template.content, true);

        content.querySelector('h3').textContent = fullName;
        content.querySelector('img').src = avatarUrl;
        content.querySelector('small').textContent = position;

        shadowRoot.appendChild(content);
    };
}

// Register custom tag to window.customElements
window.customElements.define('employee-list-item', EmployeeListItemElement);
