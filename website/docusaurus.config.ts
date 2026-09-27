import {themes as prismThemes} from 'prism-react-renderer';
import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: 'DockerComposeFluent',
  tagline: 'A fluent .NET API for generating docker-compose.yml files',
  favicon: 'img/icon.png',

  future: {
    v4: true, // Improve compatibility with the upcoming Docusaurus v4
  },

  url: 'https://seanzi86.github.io',
  baseUrl: '/DockerComposeFluent/',

  organizationName: 'seanzi86',
  projectName: 'DockerComposeFluent',

  onBrokenLinks: 'throw',
  markdown: {
    hooks: {
      onBrokenMarkdownLinks: 'throw',
    },
  },

  i18n: {
    defaultLocale: 'en',
    locales: ['en'],
  },

  presets: [
    [
      'classic',
      {
        docs: {
          path: 'docs',
          routeBasePath: '/',
          sidebarPath: './sidebars.ts',
          editUrl: 'https://github.com/seanzi86/DockerComposeFluent/tree/main/website/',
        },
        blog: false,
        theme: {
          customCss: './src/css/custom.css',
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    image: 'img/icon.png',
    colorMode: {
      respectPrefersColorScheme: true,
    },
    navbar: {
      title: 'DockerComposeFluent',
      logo: {
        alt: 'DockerComposeFluent logo',
        src: 'img/icon.png',
      },
      items: [
        {
          type: 'docSidebar',
          sidebarId: 'docsSidebar',
          position: 'left',
          label: 'Docs',
        },
        {
          href: 'https://www.nuget.org/packages/DockerComposeFluent',
          label: 'NuGet',
          position: 'right',
        },
        {
          href: 'https://github.com/seanzi86/DockerComposeFluent',
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Docs',
          items: [
            {
              label: 'Getting started',
              to: '/getting-started',
            },
          ],
        },
        {
          title: 'Project',
          items: [
            {
              label: 'GitHub',
              href: 'https://github.com/seanzi86/DockerComposeFluent',
            },
            {
              label: 'NuGet package',
              href: 'https://www.nuget.org/packages/DockerComposeFluent',
            },
            {
              label: 'Issues & roadmap',
              href: 'https://github.com/seanzi86/DockerComposeFluent/milestones',
            },
          ],
        },
      ],
      copyright: `Copyright © ${new Date().getFullYear()} Sean Carrington. Released under the MIT License.`,
    },
    prism: {
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
      additionalLanguages: ['csharp', 'yaml'],
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
