// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  apiBaseUrl: 'https://localhost:5001/api',
  googleClientId: '762723070255-vqbqdeo7sh4ocd3g7vghb5p4fl4avm1a.apps.googleusercontent.com',
  paging: {
    defaultPageSize: 10,
    lengthMenu: [10, 15, 25, 50, 100]
  },
  dateTimeFormat: {
    date: 'yyyy-MM-dd',
    dateTime: 'yyyy-MM-dd h:mm:ss a'
  }
};
