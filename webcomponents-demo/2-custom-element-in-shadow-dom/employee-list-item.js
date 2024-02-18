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

        shadowRoot.innerHTML = `
    	<style>
    	    h3 {
              color: navy;
            }
    	
            img {
                max-width: 150px;
              border-radius: 50%;
              box-shadow: 0 3px 5px rgba(0,0,0, 0.5);
              display: block;
            }
        </style>
        <h3>${fullName}</h3>
        <img src="${avatarUrl}" alt="${fullName}, ${position}" />
        <small>${position}</small>`;
    };
}

// Register custom tag to window.customElements
window.customElements.define('employee-list-item', EmployeeListItemElement);
