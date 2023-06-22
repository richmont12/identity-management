import NextAuth, { CallbacksOptions, Session } from "next-auth";
import IdentityServer4Provider from "next-auth/providers/identity-server4";

const oidcDomain = process.env.NEXTAUTH_PROVIDER_OIDC_DOMAIN;
const oidcClientId = process.env.NEXTAUTH_PROVIDER_OIDC_CLIENT_ID;
const oidcClientSecret = process.env.NEXTAUTH_PROVIDER_OIDC_CLIENT_SECRET;
const nextAuthSecret = process.env.NEXTAUTH_SECRET;

const callBackOptions: Partial<CallbacksOptions> = {
  signIn: () => true,
  jwt: ({ token, account }) => {
    if (account !== undefined && account !== null) {
      token.access_token = account.access_token;
    }
    return token;
  },
  session: ({ session: nextAuthSession, token }) => {
    const session: Session = {
      ...nextAuthSession,
      //accessToken: token.access_token,
    };
    return session;
  },
};

export const authOptions = {
  providers: [
    IdentityServer4Provider({
      id: "identity-provider",
      name: "Identity Provider",
      issuer: `https://${oidcDomain}`,
      clientId: oidcClientId,
      clientSecret: oidcClientSecret,
      authorization: {
        params: {
          scope:
            "openid profile dataapi1",
          grant_type: "authorization_code",
          authorizationUrl: `https://${oidcDomain}/connect/authorize?response_type=code`,
        },
      },
    }),
  ],
  secret: nextAuthSecret,
  debug: true,
  callbacks: callBackOptions,
}

export default NextAuth(authOptions);
