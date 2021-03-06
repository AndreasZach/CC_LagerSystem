describe('basicFuntionality', () => {
    const PIZZA_ORDER_BASE_URL ="http://localhost:7500/";
    const STORAGE_API_BASE_URL = "http://localhost:7501/";
    let currentIndex;

    it('Should load the StorageApi mainpage', () => {
        cy.visit(STORAGE_API_BASE_URL);
        cy.url().should('eq', STORAGE_API_BASE_URL);
    });
    it('Should add 1 "Ost" to the storage', () => {
        cy.get("#Add-Ost-amount").type(1);
        cy.get('#Add-Ost-submit').click();
        cy.reload();
        cy.get('#Ost-current-amount').contains('1')
    });
    it('Should add 1 "Tomatsås" to the storage', () => {
        cy.get("#Add-Tomatsås-amount").type(1);
        cy.get('#Add-Tomatsås-submit').click();
        cy.reload();
        cy.get('#Tomatsås-current-amount').contains('1')
    });
    it('Should create a pizza order through Ajax successfully', () => {
        cy.request('POST', PIZZA_ORDER_BASE_URL + 'Orders', [ "Margarita" ])
        .then((response) => {
          expect(response.status).to.eq(200)
          currentIndex = response.body.id
        });
    });
    it('Should confirm a pizza order through Ajax successfully', () => {
        cy.request({
            method: 'PUT',
            url: PIZZA_ORDER_BASE_URL + 'Confirm?orderId=' + currentIndex,
        }).then((response) => {
          expect(response.status).to.eq(200)
        })
    });
    it('Should ensure "Ost" has been removed from storage', () => {
        cy.reload();
        cy.get('#Ost-current-amount').contains('0')
    });
    it('Should ensure "Tomatsås" has been removed from storage', () => {
        cy.reload();
        cy.get('#Tomatsås-current-amount').contains('0')
    });
    it('Should remove the order created through Ajax successfully', () => {
        cy.request({
            method: 'DELETE',
            url: PIZZA_ORDER_BASE_URL + 'Delete?orderId=' + currentIndex,
        }).then((response) => {
          expect(response.status).to.eq(200)
        })
    });
});